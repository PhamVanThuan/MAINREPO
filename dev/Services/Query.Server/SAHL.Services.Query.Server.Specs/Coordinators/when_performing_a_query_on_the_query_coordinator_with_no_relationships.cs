using System.Collections.Generic;
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

namespace SAHL.Services.Query.Server.Specs.Coordinators
{
    public class when_performing_a_query_on_the_query_coordinator_with_no_relationships : WithFakes
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

            coordinator = new QueryCoordinator(hostContext
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

            dataModel = new TestDataModel
            {
                Id = 1,
                Description = "Banana",
            };

            testRepresentation = new TestRepresentation(linkResolver);
            representationTemplateCache
                .WhenToldTo(a => a.Get(dataModel.GetType()))
                .Return(testRepresentation);

            json = "{\"A\":1,\"B\":2}";
            halSerialiser
                .WhenToldTo(a => a.Serialise(testRepresentation, null))
                .Return(json);
        };


        private Because of = () =>
        {
            result = coordinator.Execute(null, () => dataModel);
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
            result.ShouldEqual(json);
        };

        private It should_have_retrieved_a_representation_template_from_the_cache = () =>
        {
            representationTemplateCache
                .WasToldTo(a => a.Get(dataModel.GetType()))
                .OnlyOnce();
        };

        private It should_have_performed_a_value_injection = () =>
        {
            valueInjecter
                .WasToldTo(a => a.Inject(testRepresentation, dataModel))
                .OnlyOnce();
        };

        private It should_have_serialised_the_injected_representation = () =>
        {
            halSerialiser
                .WasToldTo(a => a.Serialise(testRepresentation, null))
                .OnlyOnce();
        };

        private static QueryCoordinator coordinator;
        private static IHostContext hostContext;
        private static IAbsoluteUrlBuilder absoluteUrlBuilder;
        private static IRepresentationTemplateCache representationTemplateCache;
        private static IHalSerialiser halSerialiser;
        private static TestDataModel dataModel;
        private static string result;
        private static TestRepresentation testRepresentation;
        private static string json;
        private static IValueInjecter valueInjecter;
        private static IIncludeRelationshipCoordinator includeRelationshipCoordinator;
        private static ILinkResolver linkResolver;
        private static IUrlParameterSubstituter urlParameterSubstituter;
        private static string resultTrimmed;
        private static IFindQuery query;
        private static IPagingCoordinator pagingCoordinator;
        private static IQueryFactory queryFactory;
    }
}
