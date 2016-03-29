using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.PropertyDomain;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.PropertyDomain.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("PropertyDomainServiceUrlConfiguration")
                                                   .Ctor<string>("serviceName").Is("PropertyDomainService");

            For<IPropertyDomainServiceClient>().Use<PropertyDomainServiceClient>()
                                       .Ctor<IServiceUrlConfigurationProvider>()
                                            .Is(x => x.TheInstanceNamed("PropertyDomainServiceUrlConfiguration"))
                                       .Ctor<IJsonActivator>("jsonActivator")
                                            .Is<JsonActivator>();
        }
    }
}