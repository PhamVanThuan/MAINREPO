using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.DomainQuery;
using StructureMap.Configuration.DSL;

namespace SAHL.Services.DomainQuery.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("DomainQueryUrlConfiguration")
                                                   .Ctor<string>("serviceName").Is("DomainQueryService");

            For<IDomainQueryServiceClient>().Use<DomainQueryServiceClient>()
                                       .Ctor<IServiceUrlConfigurationProvider>()
                                            .Is(x => x.TheInstanceNamed("DomainQueryUrlConfiguration"))
                                       .Ctor<IJsonActivator>("jsonActivator")
                                            .Is<JsonActivator>();
        }
    }
}