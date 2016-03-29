using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Compilers;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.Parsers.Elemets;

namespace SAHL.Services.Query.DataManagers
{
    public class QueryServiceDataManager <TDataModel, TSqlStatement> : IQueryServiceDataManager
        where TDataModel : IQueryDataModel
        where TSqlStatement : ISqlStatement<TDataModel>
    {
        private readonly IDbFactory dbFactory;
        private readonly IDataModelCoordinator dataModelCoordinator;
        private readonly FieldExclusionCoordinator<TDataModel> fieldExclusionCoordinator;

        public QueryServiceDataManager(IDbFactory dbFactory, IDataModelCoordinator dataModelCoordinator)
        {
            this.dbFactory = dbFactory;
            this.dataModelCoordinator = dataModelCoordinator;
            this.fieldExclusionCoordinator = new FieldExclusionCoordinator<TDataModel>();
        }

        public IEnumerable<IQueryDataModel> GetList(IFindQuery findManyQuery)
        {
            DynamicParameters dbArgs;
            string sqlStatement;

            GetSqlStatementAndParameters(findManyQuery, out dbArgs, out sqlStatement);

            using (var dbContext = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var list = dbContext.Select<TDataModel>(sqlStatement, dbArgs);
                var castList = (IEnumerable<TDataModel>)this.dataModelCoordinator.ResolveDataModelRelationships((IEnumerable<IQueryDataModel>)list);
                return (IEnumerable<IQueryDataModel>) fieldExclusionCoordinator.MarkListItemsWithNull(castList, findManyQuery.Fields);
            }
        }

        public IQueryDataModel GetById(string id, IFindQuery findManyQuery)
        {
            AddIdSelectToWhereClause(id, findManyQuery);
            return GetList(findManyQuery).FirstOrDefault();
        }

        public IQueryDataModel GetOne(IFindQuery findManyQuery)
        {
            AddTopOneToSelect(findManyQuery);
            var attorneys = GetList(findManyQuery);
            return attorneys.FirstOrDefault();
        }

        public int GetCount(IFindQuery findManyQuery)
        {
            DynamicParameters dbArgs;
            string sqlStatement;

            GetCountSqlStatementAndParameters(findManyQuery, out dbArgs, out sqlStatement);
            using (var dbContext = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return dbContext.SelectOne<int>(sqlStatement, dbArgs);
            }
        }

        private void GetSqlStatementAndParameters(IFindQuery findManyQuery, out DynamicParameters dbArgs, out string sqlStatement)
        {
            var sqlCompiler = CreateCustomSqlCompiler();
            sqlStatement = sqlCompiler.PrepareQuery(findManyQuery);
            dbArgs = sqlCompiler.PerpareDynamicParameters(findManyQuery);
        }
        
        private void GetCountSqlStatementAndParameters(IFindQuery findManyQuery, out DynamicParameters dbArgs, out string sqlStatement)
        {
            var sqlCompiler = CreateCustomSqlCompiler();
            sqlStatement = sqlCompiler.PrepareCountQuery(findManyQuery);
            dbArgs = sqlCompiler.PerpareDynamicParameters(findManyQuery);
        }

        private CustomSqlCompiler<TDataModel> CreateCustomSqlCompiler()
        {
            return new CustomSqlCompiler<TDataModel>((ISqlStatement<TDataModel>) Activator.CreateInstance(typeof(TSqlStatement)), (TDataModel) Activator.CreateInstance(typeof(TDataModel)));
        }

        private void AddIdSelectToWhereClause(string id, IFindQuery findManyQuery)
        {
            findManyQuery.Where.Add(new WherePart()
            {
                ClauseOperator = "and",
                Field = "Id",
                ParameterName = "Id",
                Operator = "=",
                Value = id
            });
        }

        private void AddTopOneToSelect(IFindQuery findManyQuery)
        {
            findManyQuery.Limit = new LimitPart()
            {
                Count = 1
            };
        }
    }
}
