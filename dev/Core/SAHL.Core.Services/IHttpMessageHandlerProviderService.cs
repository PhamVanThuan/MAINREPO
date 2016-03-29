using System.Net.Http;

namespace SAHL.Core.Services
{
    public interface IHttpMessageHandlerProviderService
    {
        HttpMessageHandler GetMessageHandler();
    }
}