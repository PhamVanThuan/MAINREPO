using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.Query;
using SAHL.Services.Query.QueryHandlers;
using StructureMap.Configuration.DSL;
using System.Net.Http;

namespace SAHL.Config.Services.Query.Server
{
    public class WebHttpClientRegistry : Registry
    {
        public WebHttpClientRegistry()
        {
            this.For<IWebHttpClient>()
                .Use<WebHttpClient>()
                .Ctor<HttpClientHandler>("handler")
                .Is(new HttpClientHandler
                {
                    UseDefaultCredentials = true,
                });

            this.For<IServiceQueryHandler<QueryServiceQuery>>()
                .Singleton()
                .Use<QueryServiceQueryHandler>()
                .Ctor<IServiceUrlConfigurationProvider>()
                .Is(a => a.TheInstanceNamed(ServiceUrlConfigurationRegistry.ServiceUrlConfigurationName));
        }
    }
}