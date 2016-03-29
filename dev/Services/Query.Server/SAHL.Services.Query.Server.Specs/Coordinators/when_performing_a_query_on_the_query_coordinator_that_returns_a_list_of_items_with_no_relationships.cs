using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Omu.ValueInjecter;
using SAHL.Core.Identity;
using SAHL.Core.Web.Identity;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Serialiser;
using SAHL.Services.Query.Server.Specs.Fakes;
using SAHL.Services.Query.Url;
using TestListRepresentation = SAHL.Services.Query.Controllers.Test.TestListRepresentation;
using TestRepresentation = SAHL.Services.Query.Controllers.Test.TestRepresentation;

namespace SAHL.Services.Query.Server.Specs.Coordinators
{
    public class when_performing_a_query_on_the_query_coordinator_that_returns_a_list_of_items_with_no_relationships : WithFakes
    {
        Establish that = () =>
        {
            hostContext = An<HttpHostContext>();
            absoluteUrlBuilder = An<IAbsoluteUrlBuilder>();
            representationTemplateCache = An<IRepresentationTemplateCache>();
            halSerialiser = An<IHalSerialiser>();
            valueInjecter = An<IValueInjecter>();
            includeRelationshipCoordinator = An<IIncludeRelationshipCoordinator>();
            linkResolver = An<ILinkResolver>();
            urlParameterSubstituter = An<IUrlParameterSubstituter>();

            queryFactory = An<IQueryFactory>();

            coordinator = new QueryCoordinatorForTesting(hostContext
                , absoluteUrlBuilder
                , representationTemplateCache
                , halSerialiser
                , valueInjecter
                , includeRelationshipCoordinator
                , linkResolver
                , urlParameterSubstituter
                , pagingCoordinator
                , queryFactory
                );

            itemDataModel = new TestDataModel
            {
                Id = 1,
                Description = "Banana",
            };
            listDataModel = new List<TestDataModel>
            {
                itemDataModel
            };

            testListRepresentation = new TestListRepresentation(linkResolver, new List<WebApi.Hal.Representation>());
            representationTemplateCache
                .WhenToldTo(a => a.Get(typeof(IEnumerable<TestDataModel>)))
                .Return(testListRepresentation);

            testRepresentation = new TestRepresentation(linkResolver);
            representationTemplateCache
                .WhenToldTo(a => a.Get(typeof(TestDataModel)))
                .Return(testRepresentation);

            linkResolver.GetRel(testRepresentation).Returns("test");

            listJson = @"{_links:{self:{href:""some.list.url""}},A:1,B:2}";
            halSerialiser
                .WhenToldTo(a => a.Serialise(testListRepresentation, null))
                .Return(listJson);

            json = @"{_links:{self:{href:""some.item.url""}},c:3,d:4}";
            halSerialiser
                .WhenToldTo(a => a.Serialise(testRepresentation, null))
                .Return(json);

            expectedJson = @"{""_links"":{""self"":{""href"":""some.list.url""},""test"":[{""href"":""some.item.url""}]}"
                + @",""A"":1,""B"":2,""_embedded"":{""test"":[{""_links"":{""self"":{""href"":""some.item.url""}},""c"":3,""d"":4}]}}";

            query = An<IFindQuery>();
        };


        private Because of = () =>
        {
            result = coordinator.Execute(query, () => listDataModel, () => listDataModel.Count);
        };

        private It should_have_returned_a_non_null_result = () =>
        {
            result.ShouldNotBeNull();
        };

        private It should_have_returned_a_non_empty_result = () =>
        {
            result.ShouldNotBeEmpty();
        };

        private It should_have_the_expected_serialised_result = () =>
        {
            result.ShouldEqual(expectedJson);
        };

        private It should_have_retrieved_the_list_representation_template_from_the_cache = () =>
        {
            representationTemplateCache
                .WasToldTo(a => a.Get(typeof(IEnumerable<TestDataModel>)));
        };

        private It should_have_performed_a_list_value_injection = () =>
        {
            valueInjecter
                .WasToldTo(a => a.Inject(testListRepresentation, listDataModel));
        };

        private It should_have_serialised_the_list_representation = () =>
        {
            halSerialiser
                .WasToldTo(a => a.Serialise(testListRepresentation, null));
        };

        private It should_have_called_execute_on_each_item_in_the_list_data_model = () =>
        {
            var methodCall = coordinator.MethodCallParameters.Single(a => a.Key == "Execute");

            var queryParameter = methodCall.Value.Single(a => a.Key == "query").Value;
            queryParameter.ShouldNotBeNull();
            queryParameter.ShouldEqual(query);

            var modelParameter = methodCall.Value.Single(a => a.Key == "model").Value;
            modelParameter.ShouldNotBeNull();
            modelParameter.ShouldEqual(itemDataModel);
        };

        private static QueryCoordinatorForTesting coordinator;
        private static IHostContext hostContext;
        private static IAbsoluteUrlBuilder absoluteUrlBuilder;
        private static IRepresentationTemplateCache representationTemplateCache;
        private static IHalSerialiser halSerialiser;
        private static List<TestDataModel> listDataModel;
        private static string result;
        private static TestListRepresentation testListRepresentation;
        private static string listJson;
        private static IValueInjecter valueInjecter;
        private static IIncludeRelationshipCoordinator includeRelationshipCoordinator;
        private static ILinkResolver linkResolver;
        private static IUrlParameterSubstituter urlParameterSubstituter;
        private static string resultTrimmed;
        private static IFindQuery query;
        private static IPagingCoordinator pagingCoordinator;
        private static TestRepresentation testRepresentation;
        private static string json;
        private static string expectedJson;
        private static TestDataModel itemDataModel;
        private static IQueryFactory queryFactory;
    }
}
