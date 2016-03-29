using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NSubstitute.Core.Arguments;
using NUnit.Framework;
using Omu.ValueInjecter;
using SAHL.Core.Identity;
using SAHL.Core.Web.Identity;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Models.Core;
using SAHL.Services.Query.Serialiser;
using SAHL.Services.Query.Server.Specs.Fakes;
using SAHL.Services.Query.Url;
using WebApi.Hal;
using Test2Representation = SAHL.Services.Query.Controllers.Test.Test2Representation;
using TestRepresentation = SAHL.Services.Query.Controllers.Test.TestRepresentation;

namespace SAHL.Services.Query.Server.Specs.Coordinators
{
    public class when_performing_a_query_on_the_query_coordinator_with_a_one_to_one_relationship_included : WithFakes
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
                , null
                , queryFactory
                );

            dataModel = new TestDataModel
            {
                Id = 1,
                Description = "Banana",
                Relationships = new List<IRelationshipDefinition>
                {
                    new RelationshipDefinition
                    {
                        DataModelType = typeof(Test2DataModel),
                        RelatedEntity = "Test2",
                        RelatedFields = new List<IRelatedField>
                        {
                            new RelatedField
                            {
                                LocalKey = "Id",
                                RelatedKey = "TestId",
                                Value = "10",
                            }
                        },
                        RelationshipType = RelationshipType.OneToOne,
                    }
                }
            };

            relatedDataModelType = dataModel.Relationships.Single().DataModelType;

            testRepresentation = new TestRepresentation(linkResolver);
            representationTemplateCache
                .WhenToldTo(a => a.Get(dataModel.GetType()))
                .Return(testRepresentation);

            test2Representation = new Test2Representation(linkResolver);
            representationTemplateCache
                .WhenToldTo(a => a.Get(relatedDataModelType))
                .Return(test2Representation);

            currentRequestUrl = new Uri("http://somewhere/over/the/rainbow/test/1/banana");
            hostContext
                .WhenToldTo(a => a.GetCurrentRequestUrl())
                .Return(currentRequestUrl);

            applicationPath = "/over";
            hostContext
                .WhenToldTo(a => a.GetApplicationPath())
                .Return(applicationPath);

            testRepresentationHref = "/over/the/rainbow/test/1/banana";
            testRel = "test";
            testRepresentationLink = new Link(testRel, testRepresentationHref);

            linkResolver
                .WhenToldTo(a => a.GetLink(testRepresentation.GetType()))
                .Return(testRepresentationLink);

            test2RepresentationHref = "~/the/rainbow/test/1/banana/test2";
            test2Rel = "test2";
            test2RepresentationLink = new Link(test2Rel, test2RepresentationHref);

            urlParameterSubstituter.WhenToldTo(a => a.Replace(test2RepresentationHref, Arg.Any<IEnumerable<KeyValuePair<string, string>>>()))
                .Return("/the/rainbow/test/1/banana/test2");

            linkResolver
                .WhenToldTo(a => a.GetLink(test2Representation.GetType()))
                .Return(test2RepresentationLink);

            test2RelativeHref = "/the/rainbow/test/1/banana/test2";
            test2AbsoluteUrl = "http://somewhere" + "/over" + test2RelativeHref;

            absoluteUrlBuilder
                .WhenToldTo(a => a.BuildPath(test2RelativeHref, "/over"))
                .Return("/over/the/rainbow/test/1/banana/test2");

            absoluteUrlBuilder
                .WhenToldTo(a => a.BuildUrl("/over" + test2RelativeHref, currentRequestUrl))
                .Return(test2AbsoluteUrl);

            json = @"{""a"":1,""b"":2,""_links"":{""self"":{""href"":""/over/the/rainbow/test/1/banana""}"
            + @",""someOtherRel"":{""href"":""http://nowhere.url""}},""_embedded"":{""someOtherRel"":[{""e"":5,""f"":6}]}}";

            jsonWithEmbedded = @"{""a"":1,""b"":2,""_links"":{""self"":{""href"":""/over/the/rainbow/test/1/banana""},"
            + @"""someOtherRel"":{""href"":""http://nowhere.url""},""test2"":{""href"":""/over/the/rainbow/test/1/banana/test2""}}"
            + @",""_embedded"":{""someOtherRel"":[{""e"":5,""f"":6}],""test2"":[{""c"":3,""d"":4}]}}";

            halSerialiser
                .WhenToldTo(a => a.Serialise(testRepresentation, null))
                .Return(json);

            test2JsonResult = "[{c:3,d:4}]";
            includeRelationshipCoordinator
                .When(a => a.Fetch(Arg.Any<IEnumerable<LinkQuery>>()))
                .Do(a =>
                {
                    ((IEnumerable<LinkQuery>) a[0]).Single().JsonResult = test2JsonResult;
                })
                ;

            query = An<IFindQuery>();
            query.WhenToldTo(a => a.Includes).Return(new List<string>
            {
                "test2"
            });
        };

        private Because of = () =>
        {
            result = coordinator.Execute(query, () => dataModel);
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
            result.ShouldEqual(jsonWithEmbedded);
        };

        private It should_have_retrieved_a_representation_template_from_the_cache = () =>
        {
            representationTemplateCache
                .WasToldTo(a => a.Get(dataModel.GetType()));
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

        private It should_have_retrieved_a_link_for_the_included_representation = () =>
        {
            linkResolver
                .WasToldTo(a => a.GetLink(test2Representation.GetType()))
                .OnlyOnce();
        };

        private It should_have_attempted_to_build_a_url_using_the_absolute_path_and_the_current_request = () =>
        {
            absoluteUrlBuilder
                .WasToldTo(a => a.BuildUrl(testRepresentationHref + "/test2", currentRequestUrl))
                .OnlyOnce();
        };

        private It should_have_executed_a_fetch_on_the_built_queries = () =>
        {
            includeRelationshipCoordinator
                .WasToldTo(a => a.Fetch(Arg.Any<IEnumerable<LinkQuery>>()))
                .OnlyOnce();
        };

        private It should_have_performed_a_fetch_with_the_expected_url = () =>
        {
            var call = includeRelationshipCoordinator.ReceivedCalls().Single();
            var callArguments = call.GetArguments();
            var queries = callArguments.FirstOrDefault() as IEnumerable<LinkQuery>;
            var query = queries.SingleOrDefault();

            query.Relationship.ShouldBeEqualIgnoringCase(test2Rel);
            query.JsonResult.ShouldEqual(test2JsonResult);
            query.AbsoluteUrl.ShouldEqual(test2AbsoluteUrl);
        };

        private static QueryCoordinator coordinator;
        private static HttpHostContext hostContext;
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
        private static Test2Representation test2Representation;
        private static Link test2RepresentationLink;
        private static Uri currentRequestUrl;
        private static string applicationPath;
        private static string test2RepresentationHref;
        private static IUrlParameterSubstituter urlParameterSubstituter;
        private static string jsonWithEmbedded;
        private static Type relatedDataModelType;
        private static string test2Rel;
        private static string test2JsonResult;
        private static IFindQuery query;
        private static IQueryFactory queryFactory;
        private static string testRepresentationHref;
        private static Link testRepresentationLink;
        private static string test2RelativeHref;
        private static string test2AbsoluteUrl;
        private static string testRel;
    }
}
