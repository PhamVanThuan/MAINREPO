using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SAHL.Services.Interfaces.Query;

namespace SAHL.Services.Query.Connector
{
    public class QueryExecution
    {
        private readonly IQueryServiceClient QueryServiceClient;
        private readonly string url;

        public QueryExecution(IQueryServiceClient queryServiceClient, string url)
        {
            QueryServiceClient = queryServiceClient;
            this.url = url;
        }

        public dynamic Execute(string queryJson)
        {
            string url = this.url;
            if (queryJson.Length > 0)
            {
                url += "?" + queryJson;
            }
            
            QueryServiceQuery query = new QueryServiceQuery(url);
            var result = QueryServiceClient.HandleQuery(query);

            if (!result.HasErrors)
            {
                return JsonConvert.DeserializeObject<ExpandoObject>(query.Result.Results.ElementAt(0).Result);
            }

            return null;
        }
    }
}
