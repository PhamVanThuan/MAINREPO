using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using NSubstitute;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Controllers;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Models.Core;
using SAHL.Services.Query.Parsers.Elemets;
using SAHL.Services.Query.Resources.Account;
using SAHL.Services.Query.Server.Specs.Coordinators;
using SAHL.Services.Query.Server.Specs.Fakes;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Services.Query.Server.Tests.DataManagers
{
    public class when_retrieving_a_datamanager_for_a_url_that_returns_a_relation_of_a_single_item_from_a_list_representation : WithFakes
    {
        Establish that = () =>
        {
            routeMetadataCollection = An<IRouteMetadataCollection>();
            dataManagerCollection = An<IDataManagerCollection>();
            representationDataModelMapCollection = An<IRepresentationDataModelMapCollection>();

            //general setup
            findManyQuery = An<IFindQuery>();
            queryFactory = An<IQueryFactory>();
            queryFactory
                .WhenToldTo(a => a.CreateEmptyFindQuery())
                .Return(findManyQuery);

            id = "1";
            routeValues = new Dictionary<string, object>
            {
                { "id", id },
            };

            //setup for TestListRepresentation
            testListRepresentationType = typeof(TestListRepresentation);

            routeMetadataCollection
                .WhenToldTo(a => a.Get("api/tests"))
                .Return(testListRepresentationType);

            testListDataModelType = typeof (IEnumerable<TestDataModel>);
            representationDataModelMapCollection
                .WhenToldTo(a => a.Get(testListRepresentationType))
                .Return(testListDataModelType);

            testListDataManager = An<IQueryServiceDataManager>();
            dataManagerCollection
                .WhenToldTo(a => a.Get(testListDataModelType.GetGenericArguments().First()))
                .Return(testListDataManager);

            //setup for TestRepresentation
            testRepresentationType = typeof (TestRepresentation);

            routeMetadataCollection
                .WhenToldTo(a => a.Get("api/tests/{id}"))
                .Return(testRepresentationType);

            testDataModelType = typeof (TestDataModel);
            representationDataModelMapCollection
                .WhenToldTo(a => a.Get(testRepresentationType))
                .Return(testDataModelType);

            testDataManager = An<IQueryServiceDataManager>();
            dataManagerCollection
                .WhenToldTo(a => a.Get(testDataModelType))
                .Return(testDataManager);

            test2DataModel = new Test2DataModel
            {
                Id = 2,
                TestId = int.Parse(id),
                Value = "Test2",
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
                                    Value = id,
                                }
                            },
                            RelationshipType = RelationshipType.OneToOne
                        }
                }
            };

            testDataManager
                .WhenToldTo(a => a.GetById(id, Arg.Any<IFindQuery>()))
                .Return(test2DataModel);

            //setup for Test2Representation
            test2RepresentationType = typeof (Test2Representation);

            routeMetadataCollection
                .WhenToldTo(a => a.Get("api/tests/{id}/test2"))
                .Return(test2RepresentationType);

            test2DataModelType = typeof (Test2DataModel);
            representationDataModelMapCollection
                .WhenToldTo(a => a.Get(test2RepresentationType))
                .Return(test2DataModelType);

            test2DataManager = An<IQueryServiceDataManager>();
            dataManagerCollection
                .WhenToldTo(a => a.Get(test2DataModelType))
                .Return(test2DataManager);

            retriever = new DataManagerRetriever(routeMetadataCollection, representationDataModelMapCollection, dataManagerCollection, queryFactory);
        };


        private Because of = () =>
        {
            result = retriever.Get("api/tests/{id}/test2", routeValues);
        };

        private It should_have_attempted_to_retrieve_the_data_manager_for_the_data_model = () =>
        {
            dataManagerCollection.WasToldTo(a => a.Get(testListDataModelType.GetGenericArguments().First()));
        };

        private It should_have_retrieved_the_expected_data_manager = () =>
        {
            result.DataManager.ShouldEqual(test2DataManager);
        };

        private It should_have_created_an_empty_find_many_query = () =>
        {
            queryFactory.WasToldTo(a => a.CreateEmptyFindQuery());
        };

        private It should_have_retrieved_the_expected_data_model = () =>
        {
            result.FindQuery.ShouldEqual(findManyQuery);
        };

        private It should_have_the_expected_id_value_in_the_result = () =>
        {
            result.Id.ShouldEqual(id);
        };

        private It should_not_have_performed_any_calls_on_the_first_data_manager = () =>
        {
            testListDataManager.ReceivedCalls().ShouldBeEmpty();
        };

        private It should_have_performed_an_id_call_on_the_second_data_manager = () =>
        {
            testDataManager.WasToldTo(a => a.GetById(id, Arg.Any<IFindQuery>())).OnlyOnce();
        };

        private It should_not_have_performed_any_calls_on_the_returned_data_manager = () =>
        {
            test2DataManager.ReceivedCalls().ShouldBeEmpty();
        };

        private static DataManagerRetriever retriever;
        private static IRouteMetadataCollection routeMetadataCollection;
        private static IRepresentationDataModelMapCollection representationDataModelMapCollection;
        private static IDataManagerCollection dataManagerCollection;
        private static IQueryFactory queryFactory;
        private static DataManagerQueryResult result;
        private static Type testListRepresentationType;
        private static Type testListDataModelType;
        private static IQueryServiceDataManager testListDataManager;
        private static IFindQuery findManyQuery;
        private static IDictionary<string, object> routeValues;
        private static Type testRepresentationType;
        private static Type testDataModelType;
        private static IQueryServiceDataManager testDataManager;
        private static string id;
        private static Type test2RepresentationType;
        private static Type test2DataModelType;
        private static IQueryServiceDataManager test2DataManager;
        private static Test2DataModel test2DataModel;
    }
}
