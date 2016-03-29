using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System.Collections.Generic;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Templates
{
    public partial class Query
    {
        public Query(List<QueryModel> queries, QueryModel query, ServiceModel service)
        {
            this.QueryModels = queries;

            this.QueryModel = query;

            this.Service = service;
        }

        public ServiceModel Service { get; protected set; }

        public QueryModel QueryModel { get; protected set; }

        public List<QueryModel> QueryModels { get; protected set; }
    }
}