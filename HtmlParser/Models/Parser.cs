using Newtonsoft.Json;
using System.Collections.Generic;

namespace HtmlParser.Models
{
    public class Parser
    {
        public Parser()
        {
            Words = new List<Word>();
            Images = new List<Image>();
        }

        [JsonProperty(PropertyName = "words")]
        public List<Word> Words { get; set; }

        [JsonProperty(PropertyName = "images")]
        public List<Image> Images { get; set; }
    }
}