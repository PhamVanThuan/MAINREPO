using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.TextSearch;
using SAHL.Services.CapitecSearch.Services;
using SAHL.Services.Interfaces.CapitecSearch.Models;
using SAHL.Services.Interfaces.CapitecSearch.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.CapitecSearch.QueryHandlers
{
    public class ApplicationStatusQueryHandler : IServiceQueryHandler<ApplicationStatusQuery>, IServiceQueryPaginationHandler
    {
        private ITextSearchProvider textSearchProvider;
        private int currentPage;
        private int pageSize;

        public ApplicationStatusQueryHandler(ITextSearchProvider textSearchProvider)
        {
            this.textSearchProvider = textSearchProvider;
        }

        public ISystemMessageCollection HandleQuery(ApplicationStatusQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            if (!String.IsNullOrEmpty(query.IdentityNumberList))
            {
                query.IdentityNumberList = String.Format("*{0}*", query.IdentityNumberList);
            }

            var pagedQuery = new FullTextSearchPagedQueryWrapper<ApplicationStatusQuery>(query, currentPage, pageSize);
            var result = textSearchProvider.MultiFieldSearchAndAcrossExactMatchWithin<ApplicationStatusQuery, ApplicationStatusQueryResult>(pagedQuery);

            query.Result = new ServiceQueryResult<ApplicationStatusQueryResult>(result.CurrentPageResults.ToList());
            query.Result.NumberOfPages = result.Paging.TotalPages;
            query.Result.ResultCountInAllPages = result.Paging.TotalItems;
            
            if(result.CurrentPageResults.Count() > 1)
            {
                messages.AddMessage(new SystemMessage("More than one application was found for the entered criteria. Please refine your search query.", SystemMessageSeverityEnum.Error));
                return messages;
            }

            if (!String.IsNullOrEmpty(query.IdentityNumberList))
            {
                var firstResult = result.CurrentPageResults.FirstOrDefault();
                if (firstResult != null)
                {
                    var identityNumbers = firstResult.IdentityNumberList.Split(',');
                    if (!identityNumbers.Contains(query.IdentityNumberList.Replace("*", String.Empty)))
                    {
                        query.Result = new ServiceQueryResult<ApplicationStatusQueryResult>(new List<ApplicationStatusQueryResult>());
                    }
                }
            }

            return messages;
        }

        public void ReceivePaginationOptions(int currentPage, int pageSize)
        {
            if (currentPage > 0)
                this.currentPage = currentPage;

            if (pageSize > 0)
                this.pageSize = pageSize;
        }
    }
}