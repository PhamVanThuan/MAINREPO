﻿using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dapper;
using NSubstitute;
using NSubstitute.Core.Arguments;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.DataManagers.Statements;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Models.Attorney;
using SAHL.Services.Query.Parsers.Elemets;
using SAHL.Services.Query.Server.Specs.Factory;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Services.Query.Server.Tests.DataManagers
{
    public class query_service_data_manager_gets_one_attorney : WithFakes
    {
        
        private static FakeDbFactory dbFactory;
        private static QueryServiceDataManager<AttorneyDataModel, GetAttorneyStatement> queryServiceDataManager;
        private static IDataModelCoordinator dataModelCoordinator;
        private static Guid thirdPartyId;
        public static IFindQuery FindManyQuery;
        
        Establish that = () =>
        {
            dbFactory = new FakeDbFactory();
            dbFactory.NewDb().InReadOnlyAppContext().WhenToldTo(x => x.Select<AttorneyDataModel>(Arg.Any<string>(), Arg.Any<object>()))
                .Return(AttorneyRepresentationTestDataFactory.GetAttorneyDataModels());
            dataModelCoordinator = An<IDataModelCoordinator>();
            dataModelCoordinator.WhenToldTo(x => x.ResolveDataModelRelationships(Arg.Any<IEnumerable<IQueryDataModel>>()))
                .Return(AttorneyRepresentationTestDataFactory.GetAttorneyDataModels());
            queryServiceDataManager = new QueryServiceDataManager<AttorneyDataModel, GetAttorneyStatement>(dbFactory, dataModelCoordinator);
            thirdPartyId = new Guid();
            FindManyQuery = FindQueryFactory.EmpyQuery();
        };

        private Because of = () =>
        {
            queryServiceDataManager.GetOne(FindManyQuery);
        };

        private It should_call_the_database = () =>
        {
            dbFactory.NewDb().InReadOnlyAppContext().WasToldTo(x => x.Select<AttorneyDataModel>(Arg.Any<string>(), Arg.Any<DynamicParameters>()));
        };

        private It should_include_select_top_one_as_part_of_the_sql = () =>
        {

            dbFactory.NewDb()
                .InReadOnlyAppContext()
                .WasToldTo(
                    x =>
                        x.Select<AttorneyDataModel>(Param<string>.Matches(m => m.Contains("Select Top 1 * From")), Arg.Any<DynamicParameters>()));
        };

        private It should_try_to_resolve_the_relationsships = () =>
        {
            dataModelCoordinator.WasToldTo(x => x.ResolveDataModelRelationships(Arg.Any<IEnumerable<IQueryDataModel>>()));
        };
        
    }

}
