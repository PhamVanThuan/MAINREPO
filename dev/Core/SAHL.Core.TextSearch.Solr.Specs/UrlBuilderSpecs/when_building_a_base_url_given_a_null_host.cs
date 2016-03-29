using System;

using Machine.Fakes;
using Machine.Specifications;

namespace SAHL.Core.TextSearch.Solr.Specs.UrlBuilderSpecs
{
    public class when_building_a_base_url_given_a_null_host : WithFakes
    {
        private static ISolrFreeTextSearchUrlBuilder urlBuilder;
        private static string hostName;
        private static string serviceName;
        private static Exception exception;

        private Establish context = () =>
        {
            urlBuilder  = new SolrFreeTextSearchUrlBuilder();
            hostName    = "";
            serviceName = "solr";
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => { urlBuilder.BuildBaseUrl(hostName, serviceName); });
        };

        private It should_raise_an_argument_exception = () =>
        {
            exception.ShouldBeOfExactType<ArgumentNullException>();
        };

        private It should_correctly_set_the_exception_argument_name = () =>
        {
            ((ArgumentException)exception).ParamName.ShouldEqual("hostName");
        };
    }
}