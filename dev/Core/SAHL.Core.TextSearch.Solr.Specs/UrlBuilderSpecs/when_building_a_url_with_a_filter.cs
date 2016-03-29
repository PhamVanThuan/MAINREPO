using Machine.Fakes;
using Machine.Specifications;
using System.Collections.Generic;

namespace SAHL.Core.TextSearch.Solr.Specs.UrlBuilderSpecs
{
    internal class when_building_a_url_with_a_filterWithFakes
    {
        private static ISolrFreeTextSearchUrlBuilder urlBuilder;
        private static string baseUrl;
        private static string indexName;
        private static string searchTerms;
        private static string result;
        private static string expectedUrl;
        private static int pageSize;
        private static int currentPage;
        private static Dictionary<string, string> filters;

        private Establish context = () =>
        {
            urlBuilder = new SolrFreeTextSearchUrlBuilder();
            baseUrl = "http://someserver/solr";
            indexName = "clients";
            searchTerms = "John";
            pageSize = 10;
            currentPage = 1;
            filters = new Dictionary<string, string>(){
                 {"LegalEntityType", "Person"}
            };

            expectedUrl = "http://someserver/solr/clients/select?q=John&start=0&rows=10&wt=json&defType=edismax&fq=LegalEntityType:Person";
        };

        private Because of = () =>
        {
            result = urlBuilder.BuildUrl(baseUrl, indexName, searchTerms, pageSize, currentPage, filters);
        };

        private It should_correctly_build_the_base_url = () =>
        {
            result.ShouldEqual(expectedUrl);
        };
    }
}