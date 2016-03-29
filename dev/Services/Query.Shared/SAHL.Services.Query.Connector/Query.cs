using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using SAHL.Services.Interfaces.Query;
using SAHL.Services.Interfaces.Query.Connector;
using SAHL.Services.Query.Connector.Core;

namespace SAHL.Services.Query.Connector
{
    public class Query : IQuery
    {
        private readonly IQueryServiceClient QueryServiceClient;
        private readonly string url;

        private List<IOrderPart> OrderParts;
        private ILimitPart LimitPart { get; set; }
        private ISkipPart SkipPart { get; set; }
        private List<IWhereClauseOperatorPart> WhereClauseOperatorParts { get; set; }
        private List<string> includeFields { get; set; }
        private List<string> includeRelationships { get; set; }
        private IPagingPart PagingPart { get; set; }
        private QueryToJson queryToJson { get; set; }
        private QueryExecution queryExecution { get; set; }
        
        public Query(IQueryServiceClient queryServiceClient, string url)
        {
            QueryServiceClient = queryServiceClient;
            this.url = url;
            OrderParts = new List<IOrderPart>();
            includeFields = new List<string>();
            includeRelationships = new List<string>();
            WhereClauseOperatorParts = new List<IWhereClauseOperatorPart>();
            queryToJson = new QueryToJson();
            queryExecution = new QueryExecution(QueryServiceClient, url);
        }
        
        public IQuery Limit(int count)
        {

            if (LimitPart == null)
            {
                LimitPart = new LimitPart()
                {
                    Count = count
                };
            }

            return this;

        }

        public IOrderPart OrderBy(string column)
        {
            var orderQuery = new OrderPart(this, column);
            OrderParts.Add(orderQuery);
            return orderQuery;
        }

        public IQuery Skip(int count)
        {

            if (SkipPart == null)
            {
                SkipPart = new SkipPart()
                {
                    SkipCount = count
                };
            }

            return this;

        }

        public IQuery IncludeField(string fieldName)
        {

            if (includeFields.Contains(fieldName, StringComparer.OrdinalIgnoreCase))
            {
                return this;
            }
            includeFields.Add(fieldName);
            return this;
        }

        public IQuery IncludeFields(params string[] fieldNames)
        {

            foreach (string fieldName in fieldNames)
            {
                IncludeField(fieldName);
            }

            return this;

        }

        public IQuery IncludeRelationship(string fieldName)
        {
            if (includeRelationships.Contains(fieldName, StringComparer.OrdinalIgnoreCase))
            {
                return this;
            }
            includeRelationships.Add(fieldName);
            return this;
        }

        public IQuery IncludeRelationships(params string[] fieldNames)
        {

            foreach (string fieldName in fieldNames)
            {
                IncludeRelationship(fieldName);
            }

            return this;

        }

        public IWhereClauseOperatorPart Where()
        {
            var where = new WhereClauseOperatorPart(this);
            WhereClauseOperatorParts.Add(where);
            return where;
        }

        public IPagingPart IncludePaging()
        {
            PagingPart = new PagingPart(this);
            return PagingPart;
        }

        public dynamic Execute()
        {
            return queryExecution.Execute(this.AsJson());
        }

        public string AsJson()
        {
            string queryJson = "";
            string pagingJson = "";

            queryJson += queryToJson.IncludeWhereJson(WhereClauseOperatorParts, QueryToJson.ShouldIncludeSeperator(queryJson));
            queryJson += queryToJson.IncludeLimitJson(LimitPart, QueryToJson.ShouldIncludeSeperator(queryJson));
            queryJson += queryToJson.IncludeSkipJson(SkipPart, QueryToJson.ShouldIncludeSeperator(queryJson));
            queryJson += queryToJson.IncludeOrderByJson(OrderParts, QueryToJson.ShouldIncludeSeperator(queryJson));
            queryJson += queryToJson.IncludeFieldsJson(includeFields, QueryToJson.ShouldIncludeSeperator(queryJson));
            queryJson += queryToJson.IncludeRelatedItemsJson(includeRelationships, QueryToJson.ShouldIncludeSeperator(queryJson));
            queryJson = queryToJson.WrapAsFilter(queryJson);
            pagingJson = queryToJson.IncludePagingJson(PagingPart);

            return queryToJson.PrepareQuery(queryJson, pagingJson);
        }

    }
}
