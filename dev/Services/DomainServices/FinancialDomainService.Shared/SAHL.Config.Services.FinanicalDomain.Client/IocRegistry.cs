using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.FinancialDomain;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.FinanicalDomain.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("FinancialDomainServiceUrlConfiguration")
                                                   .Ctor<string>("serviceName").Is("FinancialDomainService");

            For<IFinancialDomainServiceClient>().Use<FinancialDomainServiceClient>()
                                       .Ctor<IServiceUrlConfigurationProvider>()
                                            .Is(x => x.TheInstanceNamed("FinancialDomainServiceUrlConfiguration"))
                                       .Ctor<IJsonActivator>("jsonActivator")
                                            .Is<JsonActivator>();
        }
    }
}