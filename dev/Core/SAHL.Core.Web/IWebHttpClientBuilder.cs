namespace SAHL.Core.Web
{
    public interface IWebHttpClientBuilder
    {
        IWebHttpClient GetConfiguredClient(string baseUrl);
    }
}