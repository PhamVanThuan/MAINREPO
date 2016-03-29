using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.ClientDomain;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.ClientDomain.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("ClientDomainServiceUrlConfiguration")
                                                   .Ctor<string>("serviceName").Is("ClientDomainService");

            For<IClientDomainServiceClient>().Use<ClientDomainServiceClient>()
                                       .Ctor<IServiceUrlConfigurationProvider>()
                                            .Is(x => x.TheInstanceNamed("ClientDomainServiceUrlConfiguration"))
                                       .Ctor<IJsonActivator>("jsonActivator")
                                            .Is<JsonActivator>();
        }
    }
}