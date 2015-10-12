using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HtmlParser.Library
{
    public static class WordCounter
    {
        public static readonly Regex pattern = new Regex(@"[A-Za-z']+");

        public static Dictionary<string, int> Count(string text, Regex regex)
        {
            var wc = new Dictionary<string, int>();

            foreach (Match match in regex.Matches(text))
            {
                if (!wc.ContainsKey(match.Value))
                    wc.Add(match.Value, 0);
                wc[match.Value]++;
            }

            return wc;
        }

        public static Dictionary<string, int> Count(string text)
        {
            return Count(text, pattern);
        }
    }
}