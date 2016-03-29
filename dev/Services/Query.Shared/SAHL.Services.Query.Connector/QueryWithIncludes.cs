using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using SAHL.Services.Interfaces.Query;

namespace SAHL.Services.Query.Connector
{
    public class QueryWithIncludes : IQueryWithIncludes
    {
        private readonly IQueryServiceClient QueryServiceClient;
        private readonly string url;
        private readonly QueryToJson queryToJson;
        private readonly QueryExecution queryExecution;

        private List<string> includeFields { get; set; }
        private List<string> includeRelationships { get; set; }

        public QueryWithIncludes(IQueryServiceClient queryServiceClient, string url)
        {
            QueryServiceClient = queryServiceClient;
            this.url = url;
            includeFields = new List<string>();
            includeRelationships = new List<string>();
            queryToJson = new QueryToJson();
            queryExecution = new QueryExecution(QueryServiceClient, url);
        }

        public IQueryWithIncludes IncludeField(string fieldName)
        {
            includeFields.Add(fieldName);
            return this;
        }

        public IQueryWithIncludes IncludeRelationship(string fieldName)
        {
            includeRelationships.Add(fieldName);
            return this;
        }

        public IQueryWithIncludes IncludeFields(params string[] fieldNames)
        {

            foreach (string fieldName in fieldNames)
            {
                IncludeField(fieldName);
            }

            return this;

        }

        public IQueryWithIncludes IncludeRelationships(params string[] fieldNames)
        {

            foreach (string fieldName in fieldNames)
            {
                IncludeRelationship(fieldName);
            }

            return this;

        }

        public string AsJson()
        {
            string queryJson = "";
            
            queryJson += queryToJson.IncludeFieldsJson(includeFields, QueryToJson.ShouldIncludeSeperator(queryJson));
            queryJson += queryToJson.IncludeRelatedItemsJson(includeRelationships, QueryToJson.ShouldIncludeSeperator(queryJson));
            queryJson = queryToJson.WrapAsFilter(queryJson);

            return queryJson;
        }

        public dynamic Execute()
        {
            return queryExecution.Execute(this.AsJson());
        }

    }

}
