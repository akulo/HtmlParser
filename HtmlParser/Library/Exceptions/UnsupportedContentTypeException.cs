using System;

namespace HtmlParser.Library.Exceptions
{
    public class UnsupportedContentTypeException : ApplicationException
    {
        public UnsupportedContentTypeException(string contentType)
        : base(String.Format("The requested URL has unsupported content type, '{0}'.", contentType))
        {
            ContentType = contentType;
        }

        public string ContentType { get; private set; }
    }
}