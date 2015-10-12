using System.Collections.Generic;

namespace HtmlParser.Library
{
    public interface IWords<TWord>
        where TWord : class, IWord, new()
    {
        int Total { get; set; }
        List<TWord> TopWords { get; set; }
    }
}