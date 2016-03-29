using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.TextSearch;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Services.Search.QueryHandlers;

namespace SAHL.Services.Search.Specs.QueryHandlerSpecs.SearchForClient
{
    public class when_searching_for_a_client : WithFakes
    {
        private static SearchForClientQuery query;
        private static IEnumerable<SearchFilter> filters;
        private static SearchForClientQueryHandler handler;
        private static IFreeTextSearchProvider<SearchForClientQueryResult> freeTextSearchProvider;
        private static TextSearchResult<SearchForClientQueryResult> searchResult;
        private static IEnumerable<SearchForClientQueryResult> results;
        private static SearchForClientQueryResult searchForClientQueryResult;
        private static Dictionary<string, string> expectedFilters;
        private static string filterName = "Filter_Name";
        private static string filterValue = "Filter_Value";

        private Establish context = () =>
         {
             expectedFilters = new Dictionary<string, string>();
             expectedFilters.Add(filterName, filterValue);
             searchForClientQueryResult = new SearchForClientQueryResult();
             searchForClientQueryResult.LegalEntityKey = 1;
             searchForClientQueryResult.LegalName = "Bob Speed";
             searchForClientQueryResult.CellPhoneNumber = "0714033283";
             results = new List<SearchForClientQueryResult>()
             {
                searchForClientQueryResult
             };
             searchResult = new TextSearchResult<SearchForClientQueryResult>(1, 1, 1, 1, results);
             freeTextSearchProvider = An<IFreeTextSearchProvider<SearchForClientQueryResult>>();
             filters = new List<SearchFilter>()
             {
                new SearchFilter(filterName, filterValue)
             };
             query = new SearchForClientQuery("queryText", filters, "indexName");
             freeTextSearchProvider.WhenToldTo(x => x.Search(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Dictionary<string, string>>(), Arg.Any<int>(), Arg.Any<int>()))
             .Return(searchResult);
             handler = new SearchForClientQueryHandler(freeTextSearchProvider);
         };

        private Because of = () =>
         {
             handler.HandleQuery(query);
         };

        private It should_return_the_result_from_the_free_text_search = () =>
         {
             query.Result.Results.First().ShouldEqual(searchForClientQueryResult);
         };

        private It should_use_the_search_index_and_query_text_provided = () =>
         {
             freeTextSearchProvider.WasToldTo(x => x.Search(query.IndexName, query.QueryText, Arg.Any<Dictionary<string, string>>(), Arg.Any<int>(), Arg.Any<int>()));
         };

        private It should_use_the_search_filters_provided = () =>
         {
             freeTextSearchProvider.WasToldTo(x => x.Search(query.IndexName, query.QueryText, Arg.Is<Dictionary<string, string>>(
                 y => y.First().Key == expectedFilters.First().Key && y.First().Value == expectedFilters.First().Value), Arg.Any<int>(), Arg.Any<int>()));
         };
    }
}