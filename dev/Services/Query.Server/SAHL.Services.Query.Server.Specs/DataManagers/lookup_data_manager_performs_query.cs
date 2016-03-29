using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dapper;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Helpers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Interfaces.Query.Parsers.Elements;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.Helper;
using SAHL.Services.Query.Models.Lookup;
using SAHL.Services.Query.Parsers.Elemets;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Services.Query.Server.Tests.DataManagers
{
    public class lookup_data_manager_performs_query_for_a_single_lookup_item : WithFakes
    {

        private static FakeDbFactory dbFactory;
        private static LookupDataManager lookupDataManager;
        private static ILookupTypesHelper lookupTypesHelper;
        private static ISupportedLookupDataManager supportedLookupDataManager;
        private static ILookupMetaDataModel lookupMetaDataModel;

        private static string dbName = "2AM";
        private static string lookupDescription = "Description";
        private static string lookupKey = "Id";
        private static string lookupType = "lookupType";
        private static string lookupTable = "lookupTable";
        private static string schema = "dbo";
        private static IFindQuery findQuery;

        private static IEnumerable<LookupDataModel> data;
        
        Establish that = () =>
        {

            data = new List<LookupDataModel>()
            {
                new LookupDataModel()
                {
                    Id = 1,
                    Description = "Description",
                    Relationships = new List<IRelationshipDefinition>()
                }
            };

            dbFactory = new FakeDbFactory();
            dbFactory.NewDb().InReadOnlyAppContext()
                .WhenToldTo(x => x.Select<LookupDataModel>(Arg.Any<string>(), Arg.Any<DynamicParameters>()))
                .Return(data);

            lookupTypesHelper = An<ILookupTypesHelper>();
            lookupMetaDataModel = new LookupMetaDataModel()
            {
                Db = dbName,
                LookupDescription = lookupDescription,
                LookupKey = lookupKey,
                LookupTable = lookupTable,
                LookupType = lookupType,
                Schema = schema
            };
            lookupTypesHelper.WhenToldTo(x => x.FindLookupMetaData("SomeLookup")).Return(lookupMetaDataModel);
            lookupDataManager = new LookupDataManager(dbFactory,lookupTypesHelper);
            findQuery = new FindManyQuery();
            findQuery.OrderBy = new List<IOrderPart>();
            findQuery.Where = new List<IWherePart>();
            findQuery.Fields =new List<string>();
        };


        private Because of = () =>
        {
            lookupDataManager.GetLookup(findQuery, dbName, schema, lookupType, lookupKey, lookupDescription, 1);
        };

        private It should_create_sqlquery_and_call_db = () =>
        {
            dbFactory.NewDb()
                .InReadOnlyAppContext()
                .WasToldTo(x => x.Select<LookupDataModel>("Select Top 1 * From (Select Id as Id, Description as Description From [2AM].[dbo].[lookupType]) As QS Where Id = @Id",
                    Arg.Any<DynamicParameters>()));
        };

        private It should_only_include_one_where_clause_parameter = () =>
        {
            dbFactory.NewDb()
                .InReadOnlyAppContext()
                .WasToldTo(x => x.Select<LookupDataModel>(Arg.Any<string>(),
                    Param<DynamicParameters>.Matches(m => m.ParameterNames.Count() == 1)));
        };

        private It should_call_the_db_with_the_correct_parametername = () =>
        {
            dbFactory.NewDb()
                .InReadOnlyAppContext()
                .WasToldTo(x => x.Select<LookupDataModel>(Arg.Any<string>(),
                    Param<DynamicParameters>.Matches(m => m.ParameterNames.FirstOrDefault(p => p.Equals("Id")) != null)));
        };


        
    }
}
