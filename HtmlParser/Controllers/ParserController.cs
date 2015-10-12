using HtmlParser.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HtmlParser.Library.Parsers;
using HtmlParser.Library.Exceptions;

namespace HtmlParser.Controllers
{
    [RoutePrefix("api/parser")]
    public class ParserController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get([FromUri] QueryParam query)
        {
            return Parse(query);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(QueryParam query)
        {
            return Parse(query);
        }

        private IHttpActionResult Parse(QueryParam query)
        {
            //make sure url query param is supplied
            if (query == null || string.IsNullOrEmpty(query.Url))
            {
                return BadRequest("Url parameter is required");
            }

            //default to 10 words
            int count = 10;

            if (query.Count > 0)
            {
                count = query.Count;
            }

            var url = query.Url;

            //append http in case url missing protocol
            if (!url.StartsWith("http") && !url.StartsWith("https"))
            {
                url = "http://" + url;
            }

            //process and validate url
            Uri uri = null;
            bool validUrl = Uri.TryCreate(url, UriKind.Absolute, out uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

            if (!validUrl)
            {
                return BadUrl(url);
            }

            //initilize HtmlParser
            var parser = new HtmlParser<Words, Word, Image>(uri);

            try
            {
                parser.Parse();
                return Ok(new Parser()
                {
                    Images = parser.GetImages(),
                    Words = parser.GetWords(count)
                });
            }
            catch (UriFormatException)
            {
                return BadUrl(url);
            }
            catch (UnsupportedContentTypeException ex)
            {
                return BadRequest(String.Format("HtmlParser does not support content type {0}", ex.ContentType));
            }
            catch (WebException ex)
            {
                //handle 404
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound(url);
                }

                return Unkonwn(url);
            }
            catch (Exception)
            {
                return Unkonwn(url);
            }
        }
        private IHttpActionResult BadUrl(string url)
        {
            return BadRequest(String.Format("{0} is an invalid URL", url));
        }

        private IHttpActionResult NotFound(string url)
        {
            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, String.Format("Could not find the requested URL: '{0}'.", url)));
        }

        private IHttpActionResult Unkonwn(string url)
        {
            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, String.Format("An unknown error occurred while parsing URL '{0}'.", url)));
        }
    }
}
