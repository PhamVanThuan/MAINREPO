using System;
using System.Collections.Generic;
using Dapper;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.DataManagers.Statements;
using SAHL.Services.Query.Models.Attorney;
using SAHL.Services.Query.Server.Specs.Factory;
using Machine.Fakes;
using Machine.Specifications;

namespace SAHL.Services.Query.Server.Tests.DataManagers
{
    public class query_service_data_manager_gets_attorney : WithFakes
    {
        Establish that = () =>
        {
            dbFactory = new FakeDbFactory();
            dbFactory.NewDb().InReadOnlyAppContext().WhenToldTo(x => x.Select<AttorneyDataModel>(Arg.Any<string>(), Arg.Any<object>()))
                .Return(AttorneyRepresentationTestDataFactory.GetAttorneyDataModels());
            dataModelCoordinator =  An<IDataModelCoordinator>();
            dataModelCoordinator.WhenToldTo(x => x.ResolveDataModelRelationships(Arg.Any<IEnumerable<IQueryDataModel>>()))
                .Return(AttorneyRepresentationTestDataFactory.GetAttorneyDataModels());
            queryServiceDataManager = new QueryServiceDataManager<AttorneyDataModel, GetAttorneyStatement>(dbFactory, dataModelCoordinator);
            thirdPartyId = new Guid();
            FindManyQuery = FindQueryFactory.EmpyQuery();
        };

        private Because of = () =>
        {
            queryServiceDataManager.GetById(thirdPartyId.ToString(), FindManyQuery);
        };

        private It should_call_the_database = () =>
        {
            dbFactory.NewDb().InReadOnlyAppContext().WasToldTo(x => x.Select<AttorneyDataModel>(Arg.Any<string>(), Arg.Any<DynamicParameters>()));
        };

        private It should_include_id_as_part_of_where_clause = () =>
        {
            dbFactory
                .NewDb()
                .InReadOnlyAppContext()
                .WasToldTo(x => x.Select<AttorneyDataModel>(
                        Param<string>.Matches(m => m.Contains("Where Id = @Id")), Arg.Any<DynamicParameters>()
                    )
                );
        };

        private It should_try_to_resolve_the_relationsships = () =>
        {
            dataModelCoordinator.WasToldTo(x => x.ResolveDataModelRelationships(Arg.Any<IEnumerable<IQueryDataModel>>()));
        };

        private static FakeDbFactory dbFactory;
        private static QueryServiceDataManager<AttorneyDataModel, GetAttorneyStatement> queryServiceDataManager;
        private static IDataModelCoordinator dataModelCoordinator;
        private static Guid thirdPartyId;
        public static IFindQuery FindManyQuery;
    }
}
