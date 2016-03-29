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
    public class when_handling_query_given_no_IDNumberList : WithFakes
    {
        static ITextSearchProvider textSearchProvider;
        static ApplicationStatusQuery query;
        static ApplicationStatusQueryHandler handler;
        static ISystemMessageCollection messages;
        static QueryResult<ApplicationStatusQueryResult> searchResult;

        Establish context = () =>
        {
            textSearchProvider = An<ITextSearchProvider>();

            List<ApplicationStatusQueryResult> mockresults = new List<ApplicationStatusQueryResult>() { };
            searchResult = new QueryResult<ApplicationStatusQueryResult>();

            searchResult.Paging.TotalItems = mockresults.Count;
            searchResult.Paging.ItemsPerPage = 10;
            searchResult.CurrentPageResults = mockresults;

            textSearchProvider.WhenToldTo(x => x.MultiFieldSearchAndAcrossExactMatchWithin<ApplicationStatusQuery, ApplicationStatusQueryResult>(Param.IsAny<FullTextSearchPagedQueryWrapper<ApplicationStatusQuery>>()))
                              .Return(searchResult);

            query = new ApplicationStatusQuery();

            query.IdentityNumberList = null;

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
