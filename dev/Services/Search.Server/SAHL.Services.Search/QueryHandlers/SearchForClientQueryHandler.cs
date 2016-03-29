using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.TextSearch;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;
using System.Linq;

namespace SAHL.Services.Search.QueryHandlers
{
    public class SearchForClientQueryHandler : IServiceQueryHandler<SearchForClientQuery>, IServiceQueryPaginationHandler
    {
        private int currentPage;
        private int pageSize;
        private IFreeTextSearchProvider<SearchForClientQueryResult> freeTextSearchProvider;

        public SearchForClientQueryHandler(IFreeTextSearchProvider<SearchForClientQueryResult> freeTextSearchProvider)
        {
            this.freeTextSearchProvider = freeTextSearchProvider;
        }

        public ISystemMessageCollection HandleQuery(SearchForClientQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            // perform the full text search using the passed in pagination options from below

            var filters = query.Filters.ToDictionary(k => k.Name, v => { return v.Value.Contains(" ") ? string.Format("\"{0}\"", v.Value) : v.Value; });
            TextSearchResult<SearchForClientQueryResult> results = freeTextSearchProvider.Search(query.IndexName, query.QueryText, filters, this.pageSize, this.currentPage);
            if (results != null)
            {
                query.Result = new ServiceQueryResult<SearchForClientQueryResult>(results.Results);
                query.Result.NumberOfPages = results.NumberOfPages;
                query.Result.ResultCountInPage = this.pageSize;
                query.Result.ResultCountInAllPages = results.NumberOfResults;
                query.Result.QueryDurationInMilliseconds = results.QueryTimeInMilliseconds;
            }
            else
            {
                messages.AddMessage(new SystemMessage("An error occurred while searching", SystemMessageSeverityEnum.Exception));
            }
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