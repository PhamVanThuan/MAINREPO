using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Linq;
using Dapper;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Helpers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Interfaces.Query.Parsers.Elements;
using SAHL.Services.Query.Compilers;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.DataManagers.Statements;
using SAHL.Services.Query.DataManagers.Statements.Lookup;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Models.Lookup;
using SAHL.Services.Query.Parsers.Elemets;

namespace SAHL.Services.Query.DataManagers
{
    public class LookupDataManager : ILookupDataManager
    {
        private readonly IDbFactory dbFactory;
        private ILookupTypesHelper LookupTypesHelper { get; set; }

        public LookupDataManager(IDbFactory dbFactory, ILookupTypesHelper lookupTypesHelper)
        {
            this.dbFactory = dbFactory;
            LookupTypesHelper = lookupTypesHelper;
        }

        public IEnumerable<ILookupDataModel> GetLookups(IFindQuery findManyQuery, string database, string schema, string lookupType, string keyColumn, string descriptionColumn)
        {
            DynamicParameters dbArgs;
            string sqlStatement;
            var query = new GetLookupsStatement(database, lookupType, schema, keyColumn, descriptionColumn);

            GetSqlStatementAndParameters(findManyQuery, query, lookupType, out dbArgs, out sqlStatement);
            using (var dbContext = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var lookupList = dbContext.Select<LookupDataModel>(sqlStatement, dbArgs);
                FieldExclusionCoordinator<LookupDataModel> coordinator = new FieldExclusionCoordinator<LookupDataModel>();
                return coordinator.MarkListItemsWithNull(lookupList, findManyQuery.Fields);
            }
        }

        public ILookupDataModel GetLookup(IFindQuery findManyQuery, string database, string schema, string lookupType, string keyColumn, string descriptionColumn, int keyValue)
        {
            AddIdSelectToWhereClause(keyValue, findManyQuery);
            IEnumerable<ILookupDataModel> lookups = GetLookups(findManyQuery, database, schema, lookupType, keyColumn, descriptionColumn);
            return lookups.FirstOrDefault();
        }

        private void GetSqlStatementAndParameters(IFindQuery findManyQuery, ISqlStatement<LookupDataModel> statement, string lookupType, out DynamicParameters dbArgs, out string sqlStatement)
        {
            CustomSqlCompiler<LookupDataModel> sqlCompiler = new CustomSqlCompiler<LookupDataModel>(statement, InitialiseLookupModel(lookupType));
            sqlStatement = sqlCompiler.PrepareQuery(findManyQuery);
            dbArgs = sqlCompiler.PerpareDynamicParameters(findManyQuery);
        }

        private LookupDataModel InitialiseLookupModel(string lookupType)
        {
            return new LookupDataModel();
        }

        private void AddIdSelectToWhereClause(int id, IFindQuery findManyQuery)
        {
            findManyQuery.Where.Add(new WherePart()
            {
                ClauseOperator = "and",
                Field = "Id",
                Operator = "=",
                ParameterName = "Id",
                Value = id.ToString()
            });

            findManyQuery.Limit = new LimitPart()
            {
                Count = 1
            };
        }
    }
}
