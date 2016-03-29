using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.TextSearch;
using SAHL.Services.CapitecSearch.QueryHandlers;
using SAHL.Services.CapitecSearch.Services;
using SAHL.Services.Interfaces.CapitecSearch.Models;
using SAHL.Services.Interfaces.CapitecSearch.Queries;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.CapitecSearch.Server.Specs.QueryHandlers.ApplicationStatusQueryHandlerSpecs
{
    public class when_handling_query_given_IDNumberList_and_one_result : WithFakes
    {
        static ITextSearchProvider textSearchProvider;
        static ApplicationStatusQuery query;
        static ApplicationStatusQueryHandler handler;
        static ISystemMessageCollection messages;
        static QueryResult<ApplicationStatusQueryResult> searchResult;

        static string IDNumberList;

        Establish context = () =>
        {
            IDNumberList = "8001015000219,8001015000219";
            textSearchProvider = An<ITextSearchProvider>();

            List<ApplicationStatusQueryResult> mockresults = new List<ApplicationStatusQueryResult>() 
                        {
                            new ApplicationStatusQueryResult()
                            {
                                IdentityNumberList = IDNumberList
                            }
                        };
            searchResult = new QueryResult<ApplicationStatusQueryResult>();

            searchResult.Paging.TotalItems = mockresults.Count;
            searchResult.Paging.ItemsPerPage = 10;
            searchResult.CurrentPageResults = mockresults;

            textSearchProvider.WhenToldTo(x => x.MultiFieldSearchAndAcrossExactMatchWithin<ApplicationStatusQuery, ApplicationStatusQueryResult>(Param.IsAny<FullTextSearchPagedQueryWrapper<ApplicationStatusQuery>>()))
                              .Return(searchResult);

            query = new ApplicationStatusQuery();

            query.IdentityNumberList = IDNumberList;

            handler = new ApplicationStatusQueryHandler(textSearchProvider);
            handler.ReceivePaginationOptions(1, 10);
        };

        Because of = () =>
        {
            messages = handler.HandleQuery(query);
        };

        It should_not_return_any_messages = () =>
        {
            messages.AllMessages.Count().ShouldEqual(0);
        };
    }
}
