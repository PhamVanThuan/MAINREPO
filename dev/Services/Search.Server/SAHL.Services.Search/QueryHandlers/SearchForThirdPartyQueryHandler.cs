using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.TextSearch;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;
using System.Linq;

namespace SAHL.Services.Search.QueryHandlers
{
    public class SearchForThirdPartyQueryHandler : IServiceQueryHandler<SearchForThirdPartyQuery>, IServiceQueryPaginationHandler
    {
        private int currentPage;
        private int pageSize;
        private IFreeTextSearchProvider<SearchForThirdPartyQueryResult> freeTextSearchProvider;

        public SearchForThirdPartyQueryHandler(IFreeTextSearchProvider<SearchForThirdPartyQueryResult> freeTextSearchProvider)
        {
            this.freeTextSearchProvider = freeTextSearchProvider;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleQuery(SearchForThirdPartyQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            // perform the full text search using the passed in pagination options from below

            var filters = query.Filters.ToDictionary(k => k.Name, v => { return v.Value.Contains(" ") ? string.Format("\"{0}\"", v.Value) : v.Value; });
            var results = freeTextSearchProvider.Search(query.IndexName, query.QueryText, filters, this.pageSize, this.currentPage);

            query.Result = new ServiceQueryResult<SearchForThirdPartyQueryResult>(results.Results);
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