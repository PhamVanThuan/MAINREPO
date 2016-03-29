using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System.Collections.Generic;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Templates
{
    public partial class Queries
    {
        public Queries(List<QueryModel> queries, ServiceModel service)
        {
            this.QueryModels = queries;
            this.Service = service;
        }

        public ServiceModel Service { get; protected set; }

        public List<QueryModel> QueryModels { get; protected set; }
    }
}