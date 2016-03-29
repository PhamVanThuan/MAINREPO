using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Dapper;
using Microsoft.SqlServer.Server;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Interfaces.Query.Parsers.Elements;
using SAHL.Services.Query.Parsers.Elemets;

namespace SAHL.Services.Query.Compilers
{
    public class CustomSqlCompiler<T> where T : IQueryDataModel
    {

        private ISqlStatement<T> SqlStatement { get; set; }
        private T DataModel { get; set; }

        private bool HasWhereClause { get; set; }
        public string Statement { get; set; }

        public CustomSqlCompiler(ISqlStatement<T> sqlStatement, T dataModel)
        {
            SqlStatement = sqlStatement;
            Statement = SqlStatement.GetStatement();
            DataModel = dataModel;
        }

        public string PrepareQuery(IFindQuery query)
        {
            Statement = PrepareStatement(Statement);
            Statement = WrapQuery(Statement);
            ApplyQueryLimit(query);
            ApplyQueryWhere(query);
            ApplyQueryOrderBy(query);
            ApplySkipAndTake(query);
            return Statement;
        }

        private string PrepareStatement(string statement)
        {
            //Remove any top statement included, the top is added later by the query service data manager
            //Top may be included by the developer on large tables so that the build doesn't break

            if (statement.ToLower().StartsWith("select top"))
            {
                Regex regex = new Regex(@"((Top )|(top ))\d+");
                return regex.Replace(statement, "", 1);    
            }

            return statement;
        }

        public string PrepareCountQuery(IFindQuery query)
        {
            Statement = PrepareStatement(Statement);
            ApplyCount();
            ApplyQueryWhere(query);
            return Statement;
        }

        public DynamicParameters PerpareDynamicParameters(IFindQuery query)
        {
            DynamicParameters dbArgs = new DynamicParameters();

            if (HasQueryWhere(query))
            {
                foreach (var wherePart in query.Where)
                {
                    CreateWhereParameters(wherePart, dbArgs);
                }

                return dbArgs;
            }

            return null;

        }

        public string WrapQuery(string sql)
        {
            return "Select * From (" + sql + ") As QS";
        }

        private void CreateWhereParameters(IWherePart wherePart, DynamicParameters dbArgs)
        {
            
            if (wherePart.Where == null) 
            {
                IncludeWhereParameter(wherePart, dbArgs);
            }
            else if (wherePart.Where.Count == 0)
            {
                IncludeWhereParameter(wherePart, dbArgs);
            }
            else
            {
                foreach (var where in wherePart.Where)
                {
                    CreateWhereParameters(where, dbArgs);    
                }
            }
        }

        private void IncludeWhereParameter(IWherePart wherePart, DynamicParameters dbArgs)
        {
            dbArgs.Add(wherePart.ParameterName, wherePart.Value);
        }

        private void ApplyCount()
        {
            Statement = "Select Count(*) From (" + Statement + ") As QS";
        }

        private void ApplySkipAndTake(IFindQuery query)
        {
            if (query.PagedPart != null && query.OrderBy.Count > 0)
            {
                Statement += " Offset " + (query.PagedPart.PageSize * (query.PagedPart.CurrentPage - 1)) + " Rows Fetch Next " + query.PagedPart.PageSize + " Rows Only";
            }

            if (query.Skip != null & query.OrderBy.Count > 0)
            {
                Statement += " Offset " + query.Skip.Skip + " Rows Fetch Next " + query.Skip.Take + " Rows Only";
            }
        }
        
        private void ApplyQueryWhere(IFindQuery query)
        {
            if (HasQueryWhere(query))
            {
                foreach (var wherePart in query.Where)
                {
                    IncludeWhereClause(wherePart);    
                }
                
            }
        }

        private void IncludeWhereClause(IWherePart wherePart)
        {

            if (wherePart.Where == null) 
            {
                IncludeWhereClauseItem(wherePart);
            }
            else if (wherePart.Where.Count == 0)
            {
                IncludeWhereClauseItem(wherePart);
            }
            else
            {
                foreach (var where in wherePart.Where)
                {
                    IncludeWhereClause(where);    
                }
            }
        }

        private void IncludeWhereClauseItem(IWherePart wherePart)
        {
            
            if (wherePart.Operator == "in" || wherePart.Operator == "not in")
            {
                AddClauseValueItem(wherePart.ClauseOperator, wherePart.Field, wherePart.Operator,  "(" + wherePart.Value + ")");
            }
            else
            {
                AddClauseParameterItem(wherePart.ClauseOperator, wherePart.Field, wherePart.Operator, wherePart.ParameterName);
            }
            
        }

        public void AddClauseValueItem(string clauseOperation, string fieldName, string operation, string value)
        {

            if (HasWhereClause)
            {
                Statement += " " + clauseOperation + " " + fieldName + " " + operation + " " + value;
            }
            else
            {
                Statement += " Where " + fieldName + " " + operation + " " + value;
                HasWhereClause = true;
            }
                        
        }

        public void AddClauseParameterItem(string clauseOperation, string fieldName, string operation, string parameterName)
        {
            if (HasWhereClause)
            {
                Statement += " " + clauseOperation + " " + fieldName + " " + operation +
                             " @" + parameterName;
            }
            else
            {
                Statement += " Where " + fieldName + " " + operation + " @" + parameterName;
                HasWhereClause = true;
            }

        }

        private void ApplyQueryOrderBy(IFindQuery findManyQuery)
        {
            if (HasQueryOrderBy(findManyQuery))
            {
                SetQueryOrderBy(findManyQuery.OrderBy);
            }
        }

        private void SetQueryOrderBy(List<IOrderPart> orderBy)
        {
            string prefix = " Order By ";
            var orderedItems = orderBy.OrderBy(x => x.Sequence).ToList();
            foreach (var orderPart in orderedItems)
            {
                string direction = "";

                string[] fieldParts = orderPart.Field.Split(" ".ToCharArray());

                if (fieldParts.Length > 1)
                {
                    direction = fieldParts[1];
                }

                Statement += prefix + fieldParts[0] + " " + direction;
       
                prefix = ", ";
            }
        }

        private bool HasQueryOrderBy(IFindQuery findManyQuery)
        {
            return findManyQuery.OrderBy.Count > 0;
        }

        private void ApplyQueryLimit(IFindQuery findManyQuery)
        {
            if (HasQueryLimit(findManyQuery))
            {
                if (CanApplyLimit(findManyQuery))
                {
                    SetQueryLimit(findManyQuery.Limit.Count);    
                }
            }
        }

        private bool CanApplyLimit(IFindQuery findManyQuery)
        {
            //Can only apply a limit (Select Top X) if no skip has been set, or paging has been set
            return findManyQuery.Skip == null && findManyQuery.PagedPart == null;
        }

        private void SetQueryLimit(int limit)
        {
            if (limit > 0)
            {

                if (Statement.StartsWith("Select"))
                {
                    Statement = Statement.Insert(6, " Top " + limit.ToString());    
                }             
            }
        }

        private bool HasQueryLimit(IFindQuery findManyQuery)
        {
            return findManyQuery.Limit != null;
        }

        private bool HasQueryWhere(IFindQuery findManyQuery)
        {
            return findManyQuery.Where != null;
        }

    }

}