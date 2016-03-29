using SAHL.Core.Services;
using SAHL.Services.Interfaces.Search.Models;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.Search.Queries
{
    public class SearchForThirdPartyInvoicesQuery : ServiceQuery<SearchForThirdPartyInvoicesQueryResult>, ISearchServiceQuery
    {
        public SearchForThirdPartyInvoicesQuery(string queryText, IEnumerable<SearchFilter> filters, string indexName)
        {
            this.QueryText = queryText;
            this.Filters = new List<SearchFilter>(filters);
            this.IndexName = indexName;
        }

        public string QueryText { get; protected set; }

        public IEnumerable<SearchFilter> Filters { get; protected set; }

        public string IndexName { get; protected set; }
    }
}