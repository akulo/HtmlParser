using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HtmlParser.Library
{
    public static class WordCounter
    {
        public static readonly Regex pattern = new Regex(@"[A-Za-z']+");

        public static void Count(string text, Dictionary<string, int> wc, Regex regex)
        {
            foreach (Match match in regex.Matches(text))
            {
                if (!wc.ContainsKey(match.Value))
                {
                    wc.Add(match.Value, 0);
                }
                wc[match.Value]++;
            }
        }

        public static void Count(string text, Dictionary<string, int> wc)
        {
            Count(text, wc, pattern);
        }
    }
}