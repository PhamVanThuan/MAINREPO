using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.ApplicationDomain;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.ApplicationDomain.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("ApplicationDomainServiceUrlConfiguration")
                                                   .Ctor<string>("serviceName").Is("ApplicationDomainService");

            For<IApplicationDomainServiceClient>().Use<ApplicationDomainServiceClient>()
                                       .Ctor<IServiceUrlConfigurationProvider>()
                                            .Is(x => x.TheInstanceNamed("ApplicationDomainServiceUrlConfiguration"))
                                       .Ctor<IJsonActivator>("jsonActivator")
                                            .Is<JsonActivator>();
        }
    }
}