using Machine.Fakes;
using Machine.Specifications;

namespace SAHL.Core.TextSearch.Solr.Specs.UrlBuilderSpecs
{
    public class when_building_a_base_url : WithFakes
    {
        private static ISolrFreeTextSearchUrlBuilder urlBuilder;
        private static string hostName;
        private static string serviceName;
        private static string expectedUrl;
        private static string result;

        private Establish context = () =>
        {
            urlBuilder = new SolrFreeTextSearchUrlBuilder();
            hostName = "someserver";
            serviceName = "solr";
            expectedUrl = "http://someserver/solr";
        };

        private Because of = () =>
        {
            result = urlBuilder.BuildBaseUrl(hostName, serviceName);
        };

        private It should_correctly_build_the_base_url = () =>
        {
            result.ShouldEqual(expectedUrl);
        };
    }
}