using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.AddressDomain;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.AddressDomain.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("AddressDomainServiceUrlConfiguration")
                                                   .Ctor<string>("serviceName").Is("AddressDomainService");

            For<IAddressDomainServiceClient>().Use<AddressDomainServiceClient>()
                                       .Ctor<IServiceUrlConfigurationProvider>()
                                            .Is(x => x.TheInstanceNamed("AddressDomainServiceUrlConfiguration"))
                                       .Ctor<IJsonActivator>("jsonActivator")
                                            .Is<JsonActivator>();
        }
    }
}