using System.Net.Http;

namespace SAHL.Core.Services
{
    public class HttpMessageHandlerProviderService : IHttpMessageHandlerProviderService
    {
        public HttpMessageHandler GetMessageHandler()
        {
            return new HttpClientHandler();
        }
    }
}