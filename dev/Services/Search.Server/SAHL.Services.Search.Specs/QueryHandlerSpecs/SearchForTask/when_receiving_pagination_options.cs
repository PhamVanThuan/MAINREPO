using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.TextSearch;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Services.Search.QueryHandlers;

namespace SAHL.Services.Search.Specs.QueryHandlerSpecs.SearchForTask
{
    public class when_receiving_pagination_options : WithFakes
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
        private static int pageSize;
        private static int currentPage;

        private Establish context = () =>
        {
            pageSize = 10;
            currentPage = 4;
            expectedFilters = new Dictionary<string, string>();
            expectedFilters.Add(filterName, filterValue);
            searchForTaskQueryResult = new SearchForTaskQueryResult();
            searchForTaskQueryResult.InstanceId = 1234567;
            results = new List<SearchForTaskQueryResult>()
             {
                searchForTaskQueryResult
             };
            searchResult = new TextSearchResult<SearchForTaskQueryResult>(50, 10, 1, 1, results);
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
            handler.ReceivePaginationOptions(pageSize, currentPage);
            handler.HandleQuery(query);
        };

        private It should_search_with_the_correct_pagination_options = () =>
        {
            freeTextSearchProvider.WasToldTo(x => x.Search(query.IndexName, query.QueryText, Arg.Any<Dictionary<string, string>>(), pageSize, currentPage));
        };

        private It should_return_the_page_size_as_the_result_count_in_page = () =>
        {
            query.Result.ResultCountInPage.ShouldEqual(pageSize);
        };
    }
}