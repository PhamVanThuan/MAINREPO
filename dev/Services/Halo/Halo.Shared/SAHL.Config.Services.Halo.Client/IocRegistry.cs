using System.Net.Http;
using StructureMap.Configuration.DSL;

using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.Halo;

namespace SAHL.Config.Services.Halo.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            this.For<IWebHttpClient>().Use<WebHttpClient>()
                                      .Ctor<HttpClientHandler>("handler").Is(new HttpClientHandler());

            this.For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("HaloServiceUrlConfiguration")
                                                        .Ctor<string>("serviceName").Is("HaloService");

            For<IHaloServiceClient>().Use<HaloServiceClient>()
                                       .Ctor<IServiceUrlConfigurationProvider>().Is(x => x.TheInstanceNamed("HaloServiceUrlConfiguration"))
                                       .Ctor<IJsonActivator>("jsonActivator").Is<JsonActivator>()
                                       .Ctor<IWebHttpClient>("webHttpClient").IsTheDefault();
        }
    }
}
