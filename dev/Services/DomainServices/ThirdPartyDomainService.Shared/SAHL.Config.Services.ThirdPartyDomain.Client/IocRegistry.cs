using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.ThirdPartyDomain;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.ThirdPartyDomain.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("ThirdPartyDomainServiceUrlConfiguration")
                                                   .Ctor<string>("serviceName").Is("ThirdPartyDomainService");

            For<IThirdPartyDomainServiceClient>().Use<ThirdPartyDomainServiceClient>()
                                       .Ctor<IServiceUrlConfigurationProvider>()
                                            .Is(x => x.TheInstanceNamed("ThirdPartyDomainServiceUrlConfiguration"))
                                       .Ctor<IJsonActivator>("jsonActivator")
                                            .Is<JsonActivator>();
        }
    }
}
