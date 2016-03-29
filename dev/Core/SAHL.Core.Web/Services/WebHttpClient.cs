using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SAHL.Core.Web.Services
{
    public class WebHttpClient : HttpClient, IWebHttpClient
    {
        public WebHttpClient(HttpMessageHandler handler)
            : base(handler)
        {
        }

        public new Uri BaseAddress
        {
            get { return base.BaseAddress; }
            set { base.BaseAddress = value; }
        }

        public new Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return base.PostAsync(requestUri, content);
        }

        public Task<HttpResponseMessage> Get(string requestUri)
        {
            return base.GetAsync(requestUri);
        }
    }
}