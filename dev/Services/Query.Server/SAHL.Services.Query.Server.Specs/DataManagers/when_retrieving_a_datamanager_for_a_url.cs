﻿using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Controllers;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Metadata;
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
    public class when_retrieving_a_datamanager_for_a_url : WithFakes
    {
        Establish that = () =>
        {
            testRepresentationType = typeof(TestRepresentation);

            routeMetadataCollection = An<IRouteMetadataCollection>();
            routeMetadataCollection
                .WhenToldTo(a => a.Get("api/test"))
                .Return(testRepresentationType);

            testDataModelType = typeof (TestDataModel);
            representationDataModelMapCollection = An<IRepresentationDataModelMapCollection>();
            representationDataModelMapCollection
                .WhenToldTo(a => a.Get(testRepresentationType))
                .Return(testDataModelType);

            dataManager = An<IQueryServiceDataManager>();
            dataManagerCollection = An<IDataManagerCollection>();
            dataManagerCollection
                .WhenToldTo(a => a.Get(testDataModelType))
                .Return(dataManager);

            findManyQuery = An<IFindQuery>();
            queryFactory = An<IQueryFactory>();
            queryFactory
                .WhenToldTo(a => a.CreateEmptyFindQuery())
                .Return(findManyQuery);

            routeValues = new Dictionary<string, object>();

            retriever = new DataManagerRetriever(routeMetadataCollection, representationDataModelMapCollection, dataManagerCollection, queryFactory);
        };

        private Because of = () =>
        {
            result = retriever.Get("api/test", routeValues);
        };

        private It should_have_retrieved_a_data_manager = () =>
        {
            result.ShouldNotBeNull();
        };

        private It should_have_attempted_to_retrieve_a_representation_for_each_token_in_the_url = () =>
        {
            routeMetadataCollection.WasToldTo(a => a.Get("api"));
            routeMetadataCollection.WasToldTo(a => a.Get("api/test"));
        };

        private It should_have_attempted_to_retrieve_the_mapped_data_type_for_the_retrieved_representation = () =>
        {
            representationDataModelMapCollection.WasToldTo(a => a.Get(testRepresentationType));
        };

        private It should_have_attempted_to_retrieve_the_data_manager_for_the_data_model = () =>
        {
            dataManagerCollection.WasToldTo(a => a.Get(testDataModelType));
        };

        private It should_have_retrieved_the_expected_data_manager = () =>
        {
            result.DataManager.ShouldEqual(dataManager);
        };

        private It should_have_created_an_empty_find_many_query = () =>
        {
            queryFactory.WasToldTo(a => a.CreateEmptyFindQuery());
        };

        private It should_have_retrieved_the_expected_data_model = () =>
        {
            result.FindQuery.ShouldEqual(findManyQuery);
        };

        private static DataManagerRetriever retriever;
        private static IRouteMetadataCollection routeMetadataCollection;
        private static IRepresentationDataModelMapCollection representationDataModelMapCollection;
        private static IDataManagerCollection dataManagerCollection;
        private static IQueryFactory queryFactory;
        private static DataManagerQueryResult result;
        private static Type testRepresentationType;
        private static Type testDataModelType;
        private static IQueryServiceDataManager dataManager;
        private static IFindQuery findManyQuery;
        private static IDictionary<string, object> routeValues;
    }
}
