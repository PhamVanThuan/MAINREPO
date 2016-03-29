using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using NSubstitute;
using NSubstitute.Core.Arguments;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Interfaces.Query.Parsers.Elements;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.DataManagers.Statements;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Models.Attorney;
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
    public class query_service_data_manager_gets_attorney_list : WithFakes
    {
        
        private static FakeDbFactory dbFactory;
        private static QueryServiceDataManager<AttorneyDataModel, GetAttorneyStatement> queryServiceDataManager;
        private static IDataModelCoordinator dataModelCoordinator;
        private static IFindQuery FindManyQuery;
        
        Establish that = () =>
        {
            FindManyQuery = An<IFindQuery>();
            FindManyQuery.Fields = new List<string>();
            FindManyQuery.OrderBy = new List<IOrderPart>();
            dbFactory = new FakeDbFactory();
            dbFactory.NewDb().InReadOnlyAppContext().WhenToldTo(x => x.Select<AttorneyDataModel>(Arg.Any<string>(), Arg.Any<object>()))
                .Return(AttorneyRepresentationTestDataFactory.GetAttorneyDataModels());
            dataModelCoordinator = An<IDataModelCoordinator>();
            dataModelCoordinator.WhenToldTo(x => x.ResolveDataModelRelationships(Arg.Any<IEnumerable<IQueryDataModel>>()))
                .Return(AttorneyRepresentationTestDataFactory.GetAttorneyDataModels());
            queryServiceDataManager = new QueryServiceDataManager<AttorneyDataModel, GetAttorneyStatement>(dbFactory, dataModelCoordinator);
        };

        private Because of = () =>
        {
            queryServiceDataManager.GetList(FindManyQuery);
        };

        private It should_call_the_database = () =>
        {
            dbFactory.NewDb().InReadOnlyAppContext().WasToldTo(x => x.Select<AttorneyDataModel>(Arg.Any<string>(), Arg.Any<DynamicAttribute>()));
        };

        private It should_try_to_resolve_the_relationships = () =>
        {
            dataModelCoordinator.WasToldTo(x => x.ResolveDataModelRelationships(Arg.Any<IEnumerable<IQueryDataModel>>()));
        };
        
    }

}
