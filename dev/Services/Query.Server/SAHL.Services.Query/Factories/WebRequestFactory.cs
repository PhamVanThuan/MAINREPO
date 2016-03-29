using System;
using System.Net;

namespace SAHL.Services.Query.Factories
{
    //TODO: use WebHttpClient instead?
    public class WebRequestFactory : IWebRequestFactory
    {
        public WebRequest Create(Uri requestUri)
        {
            return WebRequest.Create(requestUri);
        }

        public WebRequest Create(string requestUriString)
        {
            return WebRequest.Create(requestUriString);
        }
    }
}