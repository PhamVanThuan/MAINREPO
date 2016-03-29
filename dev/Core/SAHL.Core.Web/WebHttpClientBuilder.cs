using SAHL.Core.Web.Services;
using System;
using System.Net.Http;

namespace SAHL.Core.Web
{
    public class WebHttpClientBuilder : SAHL.Core.Web.IWebHttpClientBuilder
    {
        public virtual IWebHttpClient GetConfiguredClient(string baseUrl)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var httpClient = new WebHttpClient(httpClientHandler)
            {
                BaseAddress = new Uri(baseUrl)
            };

            return httpClient;
        }
    }
}