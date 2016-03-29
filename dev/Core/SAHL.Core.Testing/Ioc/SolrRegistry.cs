using SAHL.Core.Services;
using SAHL.Core.TextSearch;
using SAHL.Core.TextSearch.Solr;
using SAHL.Core.Web;
using StructureMap.Configuration.DSL;

namespace SAHL.Core.Testing.Ioc
{
    public class SolrRegistry<T, T2, T3> : Registry
    {
        public SolrRegistry()
        {
            Scan(x =>
            {
                x.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL") && !y.FullName.Contains("Testing"));
                x.ConnectImplementationsToTypesClosing(typeof(IFreeTextSearchProvider<>));
            });

            For<IServiceUrlConfigurationProvider>()
                .Use<ServiceUrlConfigurationProvider>()
                .Named("SolrServiceUrlConfiguration")
                .Ctor<string>("serviceName").Is("SolrService");

            For<IFreeTextSearchProvider<T>>().Use<SolrFreeTextSearchProvider<T>>()
                                                   .Ctor<IServiceUrlConfigurationProvider>()
                                                        .Is(x => x.TheInstanceNamed("SolrServiceUrlConfiguration"));

            For<IFreeTextSearchProvider<T2>>().Use<SolrFreeTextSearchProvider<T2>>()
                                                   .Ctor<IServiceUrlConfigurationProvider>()
                                                        .Is(x => x.TheInstanceNamed("SolrServiceUrlConfiguration"));

            For<IFreeTextSearchProvider<T3>>().Use<SolrFreeTextSearchProvider<T3>>()
                                                   .Ctor<IServiceUrlConfigurationProvider>()
                                                        .Is(x => x.TheInstanceNamed("SolrServiceUrlConfiguration"));

            For<ISolrFreeTextSearchUrlBuilder>().Use<SolrFreeTextSearchUrlBuilder>();

            For<IWebHttpClientBuilder>().Use<WebHttpClientBuilder>();
        }
    }
}
