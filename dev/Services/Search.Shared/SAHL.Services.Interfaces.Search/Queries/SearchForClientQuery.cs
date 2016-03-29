using SAHL.Core.Services;
using SAHL.Services.Interfaces.Search.Models;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.Search.Queries
{
    public class SearchForClientQuery : ServiceQuery<SearchForClientQueryResult>, ISearchServiceQuery
    {
        public SearchForClientQuery(string queryText, IEnumerable<SearchFilter> filters, string indexName)
        {
            this.IndexName = indexName;
            this.QueryText = queryText;
            this.Filters = new List<SearchFilter>(filters);
        }

        public string QueryText { get; protected set; }

        public IEnumerable<SearchFilter> Filters { get; protected set; }

        public string IndexName { get; protected set; }
    }
}