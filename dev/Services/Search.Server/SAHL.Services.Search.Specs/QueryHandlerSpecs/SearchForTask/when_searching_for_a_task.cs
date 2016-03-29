using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.TextSearch;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Services.Search.QueryHandlers;
using System.Linq;

namespace SAHL.Services.Search.Specs.QueryHandlerSpecs.SearchForTask
{
    public class when_searching_for_a_task : WithFakes
    {
        private static SearchForTaskQuery query;
        private static IEnumerable<SearchFilter> filters;
        private static SearchForTaskQueryHandler handler;
        private static IFreeTextSearchProvider<SearchForTaskQueryResult> freeTextSearchProvider;
        private static TextSearchResult<SearchForTaskQueryResult> searchResult;
        private static IEnumerable<SearchForTaskQueryResult> results;
        private static SearchForTaskQueryResult searchForTaskQueryResult;
        private static Dictionary<string, string> expectedFilters;
        private static string filterName = "Filter_Name";
        private static string filterValue = "Filter_Value";

        private Establish context = () =>
        {
            expectedFilters = new Dictionary<string, string>();
            expectedFilters.Add(filterName, filterValue);
            searchForTaskQueryResult = new SearchForTaskQueryResult();
            searchForTaskQueryResult.InstanceId = 1234567;
            results = new List<SearchForTaskQueryResult>()
             {
                searchForTaskQueryResult
             };
            searchResult = new TextSearchResult<SearchForTaskQueryResult>(1, 1, 1, 1, results);
            freeTextSearchProvider = An<IFreeTextSearchProvider<SearchForTaskQueryResult>>();
            filters = new List<SearchFilter>()
             {
                new SearchFilter(filterName, filterValue)
             };
            query = new SearchForTaskQuery("queryText", filters, "indexName");
            freeTextSearchProvider.WhenToldTo(x => x.Search(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Dictionary<string, string>>(), Arg.Any<int>(), Arg.Any<int>()))
            .Return(searchResult);
            handler = new SearchForTaskQueryHandler(freeTextSearchProvider);
        };

        private Because of = () =>
        {
            handler.HandleQuery(query);
        };

        private It should_return_the_result_from_the_free_text_search = () =>
        {
            query.Result.Results.First().ShouldEqual(searchForTaskQueryResult);
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