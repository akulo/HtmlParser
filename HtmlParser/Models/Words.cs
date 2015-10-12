using HtmlParser.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HtmlParser.Models
{
    public class Words : IWords<Word>
    {
        public Words()
        {
            Total = 0;
            TopWords = new List<Word>();
        }

        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }

        [JsonProperty(PropertyName = "top-words")]
        public List<Word> TopWords { get; set; }
    }
}