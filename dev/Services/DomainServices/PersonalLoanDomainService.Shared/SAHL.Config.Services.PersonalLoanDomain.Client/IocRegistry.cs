using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.PersonalLoanDomain;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.PersonalLoanDomain.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("PersonalLoanDomainServiceUrlConfiguration")
                                                   .Ctor<string>("serviceName").Is("PersonalLoanDomainService");

            For<IPersonalLoanDomainServiceClient>().Use<PersonalLoanDomainServiceClient>()
                                       .Ctor<IServiceUrlConfigurationProvider>()
                                            .Is(x => x.TheInstanceNamed("PersonalLoanDomainServiceUrlConfiguration"))
                                       .Ctor<IJsonActivator>("jsonActivator")
                                            .Is<JsonActivator>();
        }
    }
}
