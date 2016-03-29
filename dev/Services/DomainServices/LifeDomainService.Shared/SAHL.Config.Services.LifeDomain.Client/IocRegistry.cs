using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.LifeDomain;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.LifeDomain.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("LifeDomainServiceUrlConfiguration")
                                                   .Ctor<string>("serviceName").Is("LifeDomainService");

            For<ILifeDomainServiceClient>().Use<LifeDomainServiceClient>()
                                       .Ctor<IServiceUrlConfigurationProvider>()
                                            .Is(x => x.TheInstanceNamed("LifeDomainServiceUrlConfiguration"))
                                       .Ctor<IJsonActivator>("jsonActivator")
                                            .Is<JsonActivator>();
        }
    }
}