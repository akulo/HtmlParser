using HtmlAgilityPack;
using HtmlParser.Library.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace HtmlParser.Library.Parsers
{
    public class HtmlParser<TWord, TImage>
        where TWord : class, IWord, new()
        where TImage : class, IImage, new()
    {
        public static readonly string ContentType = "text/html";

        public Uri Uri { get; set; }

        public HtmlDocument Document { get; set; }

        public HtmlParser(Uri uri)
        {
            this.Uri = uri;
        }

        public HtmlParser(string url)
        {
            this.Uri = new Uri(url);
        }

        public HtmlParser(Uri uri, HtmlDocument document) : this(uri)
        {
            this.Document = document;
        }

        public void Parse()
        {
            var request = WebRequest.Create(Uri);
            var response = (HttpWebResponse)request.GetResponse();

            var responseStream = response.GetResponseStream();
            var sr = new StreamReader(responseStream);
            var content = sr.ReadToEnd();

            var contentType = response.ContentType.Split(';').First();
            if (!contentType.Equals(ContentType))
            {
                throw new UnsupportedContentTypeException(contentType);
            }

            Document = new HtmlDocument();
            Document.LoadHtml(content);
        }

        public List<TImage> GetImages()
        {

            return Document.DocumentNode.Element("html")
                .Element("body")
                .SelectNodes("//img[@src]")
                .Select(image => new TImage
                {
                    Url = NormalizeImage(image.GetAttributeValue("src", null)),
                    Text = image.GetAttributeValue("alt", "")
                })
                .Where(image => !String.IsNullOrEmpty(image.Url))//filter out empty images
                .GroupBy(image => image.Url)//get distinct images by url
                .Select(g => g.First())
                .ToList();
        }

        private string NormalizeImage(string url)
        {
            if (url == null) return null;

            if (url.StartsWith("http") || url.StartsWith("https"))
            {
                return url;
            }

            if (url.StartsWith("//"))
            {
                return Uri.GetLeftPart(UriPartial.Scheme) + url.Remove(0, 2);
            }

            return new Uri(Uri, url).AbsoluteUri;
        }

        public List<TWord> GetWords(int count = 10)
        {
            var wc = new Dictionary<string, int>();

            var nodes = this.Document.DocumentNode.Element("html")
                .Element("body").DescendantsAndSelf().Where(n =>
                   n.NodeType == HtmlNodeType.Text && //only select text types
                   n.ParentNode.Name != "script" && //exculde scripts
                   n.ParentNode.Name != "style" && //exclude css styles
                   !string.IsNullOrEmpty(n.InnerText) //exclude all empty text nodes
                )
                .Select(n => HtmlEntity.DeEntitize(n.InnerText.Trim()));

            //build text string from nodes
            StringBuilder sb = nodes.Aggregate(new StringBuilder(), (s, text) => s.Append(" " + text));

            WordCounter.Count(sb.ToString(), wc);

            return wc.Select(i => new TWord { Value = i.Key, Count = i.Value })
            .OrderByDescending(w => w.Count)
            .Take(count)
            .ToList();
        }
    }
}