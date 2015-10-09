using HtmlParser.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HtmlParser.Models
{
    public class Word : IWord
    {
        [JsonProperty(PropertyName = "word")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
    }
}