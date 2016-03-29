using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.FinanceDomain;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.FinanceDomain.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("FinanceDomainServiceUrlConfiguration")
                                                   .Ctor<string>("serviceName").Is("FinanceDomainService");

            For<IFinanceDomainServiceClient>().Use<FinanceDomainServiceClient>()
                                       .Ctor<IServiceUrlConfigurationProvider>()
                                            .Is(x => x.TheInstanceNamed("FinanceDomainServiceUrlConfiguration"))
                                       .Ctor<IJsonActivator>("jsonActivator")
                                            .Is<JsonActivator>();
        }
    }
}
