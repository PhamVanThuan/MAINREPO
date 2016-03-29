using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.Search;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.Search.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("SearchServiceUrlConfiguration")
                                                   .Ctor<string>("serviceName").Is("SearchServiceService");

            For<ISearchServiceClient>().Use<SearchServiceClient>()
                                       .Ctor<IServiceUrlConfigurationProvider>()
                                            .Is(x => x.TheInstanceNamed("SearchServiceUrlConfiguration"))
                                       .Ctor<IJsonActivator>("jsonActivator")
                                            .Is<JsonActivator>();
        }
    }
}