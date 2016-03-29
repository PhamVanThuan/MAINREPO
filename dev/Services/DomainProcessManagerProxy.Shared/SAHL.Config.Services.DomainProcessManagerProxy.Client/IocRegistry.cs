using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.DomainProcessManagerProxy;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.DomainProcessManagerProxy.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("DomainProcessManagerProxyServiceUrlConfiguration")
                                                      .Ctor<string>("serviceName").Is("DomainProcessManagerProxyService");

            For<IDomainProcessManagerProxyServiceClient>().Use<DomainProcessManagerProxyServiceClient>()
                                       .Ctor<IServiceUrlConfigurationProvider>()
                                            .Is(x => x.TheInstanceNamed("DomainProcessManagerProxyServiceUrlConfiguration"))
                                       .Ctor<IJsonActivator>("jsonActivator")
                                            .Is<JsonActivator>();
        }
    }
}
