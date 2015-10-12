using Newtonsoft.Json;
using System.Collections.Generic;

namespace HtmlParser.Models
{
    public class Parser
    {
        public Parser()
        {
            Words = new Words();
            Images = new List<Image>();
        }

        [JsonProperty(PropertyName = "words")]
        public Words Words { get; set; }

        [JsonProperty(PropertyName = "images")]
        public List<Image> Images { get; set; }
    }
}