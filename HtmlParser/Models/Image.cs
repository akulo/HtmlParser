using HtmlParser.Library;
using Newtonsoft.Json;

namespace HtmlParser.Models
{
    public class Image : IImage
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }
}