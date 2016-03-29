using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.BankAccountDomain;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.BankAccountDomain.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("BankAccountDomainServiceUrlConfiguration")
                                                   .Ctor<string>("serviceName").Is("BankAccountDomainService");

            For<IBankAccountDomainServiceClient>().Use<BankAccountDomainServiceClient>()
                                       .Ctor<IServiceUrlConfigurationProvider>()
                                            .Is(x => x.TheInstanceNamed("BankAccountDomainServiceUrlConfiguration"))
                                       .Ctor<IJsonActivator>("jsonActivator")
                                            .Is<JsonActivator>();
        }
    }
}