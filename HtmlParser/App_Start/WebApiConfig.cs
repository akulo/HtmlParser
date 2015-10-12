using HtmlParser.Library;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace HtmlParser
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            //only enable JSON Negotiator
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(new JsonMediaTypeFormatter()));
        }
    }
}
 