using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.SystemMessages;
using SAHL.Core.TextSearch;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Services.Search.QueryHandlers;

namespace SAHL.Services.Search.Specs.QueryHandlerSpecs.SearchForClient
{
    public class when_the_returned_results_are_null : WithFakes
    {
        private static SearchForClientQuery query;
        private static IEnumerable<SearchFilter> filters;
        private static SearchForClientQueryHandler handler;
        private static IFreeTextSearchProvider<SearchForClientQueryResult> freeTextSearchProvider;
        private static TextSearchResult<SearchForClientQueryResult> searchResult;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
         {
             messages = new SystemMessageCollection();
             searchResult = null;
             freeTextSearchProvider = An<IFreeTextSearchProvider<SearchForClientQueryResult>>();
             filters = new List<SearchFilter>()
             {
                new SearchFilter("FilterName", "FilterValue")
             };
             query = new SearchForClientQuery("queryText", filters, "indexName");
             freeTextSearchProvider.WhenToldTo(x => x.Search(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Dictionary<string, string>>(), Arg.Any<int>(), Arg.Any<int>()))
             .Return(searchResult);
             handler = new SearchForClientQueryHandler(freeTextSearchProvider);
         };

        private Because of = () =>
         {
             messages = handler.HandleQuery(query);
         };

        private It should_return_the_result_from_the_free_text_search = () =>
         {
             query.Result.ShouldBeNull();
         };

        private It should_return_an_error_message = () =>
         {
             messages.ErrorMessages().First().Message.ShouldEqual("An error occurred while searching");
         };
    }
}