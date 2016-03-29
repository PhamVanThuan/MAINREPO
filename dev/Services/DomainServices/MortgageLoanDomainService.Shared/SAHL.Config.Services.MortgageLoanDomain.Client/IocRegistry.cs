using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.MortgageLoanDomain;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.MortgageLoanDomain.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("MortgageLoanDomainServiceUrlConfiguration")
                                                   .Ctor<string>("serviceName").Is("MortgageLoanDomainService");

            For<IMortgageLoanDomainServiceClient>().Use<MortgageLoanDomainServiceClient>()
                                       .Ctor<IServiceUrlConfigurationProvider>()
                                            .Is(x => x.TheInstanceNamed("MortgageLoanDomainServiceUrlConfiguration"))
                                       .Ctor<IJsonActivator>("jsonActivator")
                                            .Is<JsonActivator>();
        }
    }
}
