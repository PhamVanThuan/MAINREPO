using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.TextSearch;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;

namespace SAHL.Services.Search.QueryHandlers
{
    public class SearchForThirdPartyInvoicesQueryHandler : IServiceQueryHandler<SearchForThirdPartyInvoicesQuery>, IServiceQueryPaginationHandler
    {
        private int currentPage;
        private int pageSize;
        private IFreeTextSearchProvider<SearchForThirdPartyInvoicesQueryResult> freeTextSearchProvider;

        public SearchForThirdPartyInvoicesQueryHandler(IFreeTextSearchProvider<SearchForThirdPartyInvoicesQueryResult> freeTextSearchProvider)
        {
            this.freeTextSearchProvider = freeTextSearchProvider;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleQuery(SearchForThirdPartyInvoicesQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            // perform the full text search using the passed in pagination options from below

            var filters = query.Filters.ToDictionary(k => k.Name, v => { return v.Value; });
            var results = freeTextSearchProvider.Search(query.IndexName, query.QueryText, filters, this.pageSize, this.currentPage);

            query.Result = new ServiceQueryResult<SearchForThirdPartyInvoicesQueryResult>(results.Results);
            query.Result.NumberOfPages = results.NumberOfPages;
            query.Result.ResultCountInPage = this.pageSize;
            query.Result.ResultCountInAllPages = results.NumberOfResults;
            query.Result.QueryDurationInMilliseconds = results.QueryTimeInMilliseconds;
            return messages;
        }

        public void ReceivePaginationOptions(int pageSize, int currentPage)
        {
            if (currentPage > 0)
            {
                this.currentPage = currentPage;
            }

            if (pageSize > 0)
            {
                this.pageSize = pageSize;
            }
        }
    }
}