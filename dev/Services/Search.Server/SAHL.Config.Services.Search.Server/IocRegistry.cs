using SAHL.Core.Services;
using SAHL.Core.TextSearch;
using SAHL.Core.TextSearch.Solr;
using SAHL.Services.Interfaces.Search.Models;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.Search.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            Scan(x =>
            {
                x.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                x.ConnectImplementationsToTypesClosing(typeof(IFreeTextSearchProvider<>));
            });

            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("SolrServiceUrlConfiguration")
                                                   .Ctor<string>("serviceName").Is("SolrService");

            For<IFreeTextSearchProvider<SearchForClientQueryResult>>().Use<SolrFreeTextSearchProvider<SearchForClientQueryResult>>()
                                                   .Ctor<IServiceUrlConfigurationProvider>()
                                                        .Is(x => x.TheInstanceNamed("SolrServiceUrlConfiguration"));

            For<IFreeTextSearchProvider<SearchForThirdPartyQueryResult>>().Use<SolrFreeTextSearchProvider<SearchForThirdPartyQueryResult>>()
                                                   .Ctor<IServiceUrlConfigurationProvider>()
                                                        .Is(x => x.TheInstanceNamed("SolrServiceUrlConfiguration"));

            For<IFreeTextSearchProvider<SearchForTaskQueryResult>>().Use<SolrFreeTextSearchProvider<SearchForTaskQueryResult>>()
                                                   .Ctor<IServiceUrlConfigurationProvider>()
                                                        .Is(x => x.TheInstanceNamed("SolrServiceUrlConfiguration"));

            For<IFreeTextSearchProvider<SearchForThirdPartyInvoicesQueryResult>>().Use<SolrFreeTextSearchProvider<SearchForThirdPartyInvoicesQueryResult>>()
                                                   .Ctor<IServiceUrlConfigurationProvider>()
                                                   .Is(x => x.TheInstanceNamed("SolrServiceUrlConfiguration"));
        }
    }
}