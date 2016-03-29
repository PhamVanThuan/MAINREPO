using Machine.Fakes;
using Machine.Specifications;
using System.Collections.Generic;

namespace SAHL.Core.TextSearch.Solr.Specs.UrlBuilderSpecs
{
    public class when_building_a_url_with_multiple_terms_and_an_operator : WithFakes
    {
        private static ISolrFreeTextSearchUrlBuilder urlBuilder;
        private static string baseUrl;
        private static string indexName;
        private static string searchTerms;
        private static string result;
        private static string expectedUrl;
        private static int pageSize;
        private static int currentPage;

        private Establish context = () =>
        {
            urlBuilder = new SolrFreeTextSearchUrlBuilder();
            baseUrl = "http://someserver/solr";
            indexName = "clients";
            searchTerms = "John +Smith";
            pageSize = 10;
            currentPage = 1;

            expectedUrl = "http://someserver/solr/clients/select?q=John+%2bSmith&start=0&rows=10&wt=json&defType=edismax";
        };

        private Because of = () =>
        {
            result = urlBuilder.BuildUrl(baseUrl, indexName, searchTerms, pageSize, currentPage, new Dictionary<string, string>());
        };

        private It should_correctly_build_the_base_url = () =>
        {
            result.ShouldEqual(expectedUrl);
        };
    }
}