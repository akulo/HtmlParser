using Newtonsoft.Json;

namespace HtmlParser.Models
{
    public class QueryParam
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }

        [JsonProperty(PropertyName = "word-pattern")]
        public string WordPattern { get; set; }
    }
}