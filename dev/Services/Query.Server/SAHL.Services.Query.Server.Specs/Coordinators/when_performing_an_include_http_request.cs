using System;
using System.Data;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using NSubstitute;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.Factories;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Services.Query.Server.Specs.Coordinators
{
    public class when_performing_an_include_http_request : WithFakes
    {

        Establish that = () =>
        {
            webRequest = An<WebRequest>();

            webRequestFactory = An<IWebRequestFactory>();

            streamResultReaderFactory = An<IStreamResultReaderFactory>();

            coordinator = new IncludeRelationshipCoordinator(webRequestFactory, streamResultReaderFactory);

            url = new LinkQuery("rela1", "", "http://banana.url", null);
            urls = new List<LinkQuery>
            {
                url,
            };

            webRequestFactory
                .WhenToldTo(a => a.Create("http://banana.url"))
                .Return(webRequest);

            parsedResult = "banana";

            streamResultReaderFactory
                .WhenToldTo(a => a.Process(webRequest, Arg.Any<Func<StreamReader, string>>()))
                .Return(parsedResult);
        };


        private Because of = () =>
        {
            coordinator.Fetch(urls);
        };

        private It should_have_a_non_null_result = () =>
        {
            url.JsonResult.ShouldNotBeNull();
        };

        private It should_have_a_non_empty_result = () =>
        {
            url.JsonResult.ShouldNotBeEmpty();
        };

        private It should_have_a_matching_number_of_results_to_the_urls_provided = () =>
        {
            urls.Count().ShouldEqual(urls.Count);
        };

        private It should_have_created_a_request = () =>
        {
            webRequestFactory.WasToldTo(a => a.Create("http://banana.url")).OnlyOnce();
        };

        private It should_have_processed_the_response = () =>
        {
            streamResultReaderFactory
                .WasToldTo(a => a.Process(Arg.Is(webRequest), Arg.Any<Func<StreamReader, string>>()))
                .OnlyOnce();
        };

        private It should_have_returned_the_expected_value = () =>
        {
            url.JsonResult.ShouldEqual(parsedResult);
        };

        private static IWebRequestFactory webRequestFactory;
        private static IncludeRelationshipCoordinator coordinator;
        private static IStreamResultReaderFactory streamResultReaderFactory;
        private static List<LinkQuery> urls;
        private static WebRequest webRequest;
        private static string parsedResult;
        private static LinkQuery url;
    }
}
