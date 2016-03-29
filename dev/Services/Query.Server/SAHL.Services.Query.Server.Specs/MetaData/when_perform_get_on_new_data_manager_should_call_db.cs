using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web.SessionState;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.LinkCoordinator;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Parsers;
using SAHL.Services.Query.Server.Specs.Coordinators;
using SAHL.Services.Query.Server.Specs.Factory;
using SAHL.Services.Query.Server.Specs.Fakes;
using SAHL.Services.Query.Validators;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;
using TestRepresentation = SAHL.Services.Query.Controllers.Test.TestRepresentation;

namespace SAHL.Services.Query.Server.Specs.MetaData
{
    public class when_perform_get_on_new_data_manager_should_call_db : WithFakes
    {

        private static FakeDbFactory dbFactory;
        private static IDataModelCoordinator dataModelCoordinator;
        private static IDataManagerCollection collection;
        private static IQueryServiceDataManager dataManager;
        private static IQueryFactory queryFactory;
        public static IQueryStringQueryParser queryStringQueryParser;
        public static IJsonQueryParser jsonQueryParser;
        public static IPagingParser pagingParser;

        Establish that = () =>
        {

            dbFactory = new FakeDbFactory();
            dataModelCoordinator = new DataModelCoordinator();

            queryStringQueryParser = new QueryStringQueryParser();
            jsonQueryParser = new JsonQueryParser();
            pagingParser = new PagingParser();
            findQueryValidator = new FindQueryValidator();

            queryFactory = new QueryFactory(findQueryValidator, queryStringQueryParser, jsonQueryParser, pagingParser);

            var result = new Dictionary<Type, Type>();

            var type = typeof(QueryServiceDataManager<,>);
            var queryServiceDataModel = type.MakeGenericType(typeof(TestDataModel), typeof(GetTestStatement));
            result.Add(typeof(TestRepresentation), queryServiceDataModel);

            collection = new DataManagerCollection(result, dbFactory, dataModelCoordinator);
            dataManager = collection.Get(typeof (TestRepresentation));

            dbFactory.NewDb().InReadOnlyAppContext().WhenToldTo(x => x.Select<TestDataModel>(Arg.Any<string>(), Arg.Any<object>())).Return(TestDataFactory.GetTestDataModels);

        };

        private Because of = () =>
        {
            dataManager.GetById("1", queryFactory.CreateEmptyFindQuery());
        };

        private It should_perform_a_call_on_the_db = () =>
        {
            dbFactory.NewDb().InReadOnlyAppContext()
                .WasToldTo(x => x.Select<TestDataModel>(Arg.Any<string>(), Arg.Any<object>()));
        };

        private It shoud_have_the_correct_sql = () =>
        {
            dbFactory.NewDb().InReadOnlyAppContext()
                .WasToldTo(x => x.Select<TestDataModel>(Param<string>.Matches(m => m.Contains("T.Id, T.Description From Test T")), Arg.Any<object>()));
        };

        private static IFindQueryValidator findQueryValidator;
    }
}
