﻿using Machine.Fakes;
using Machine.Specifications;
using System;

namespace SAHL.Core.TextSearch.Solr.Specs.UrlBuilderSpecs
{
    public class when_building_a_base_url_given_a_null_service : WithFakes
    {
        private static ISolrFreeTextSearchUrlBuilder urlBuilder;
        private static string hostName;
        private static string serviceName;
        private static Exception exception;

        private Establish context = () =>
        {
            urlBuilder  = new SolrFreeTextSearchUrlBuilder();
            hostName    = "someserver";
            serviceName = "";
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
            ((ArgumentException)exception).ParamName.ShouldEqual("serviceName");
        };
    }
}