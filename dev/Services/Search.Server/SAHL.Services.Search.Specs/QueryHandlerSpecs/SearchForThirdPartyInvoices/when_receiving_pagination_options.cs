using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.TextSearch;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Services.Search.QueryHandlers;

namespace SAHL.Services.Search.Specs.QueryHandlerSpecs.SearchForThirdPartyInvoices
{
    public class when_receiving_pagination_options : WithFakes
    {
        private static SearchForThirdPartyInvoicesQuery query;
        private static IEnumerable<SearchFilter> filters;
        private static SearchForThirdPartyInvoicesQueryHandler handler;
        private static IFreeTextSearchProvider<SearchForThirdPartyInvoicesQueryResult> freeTextSearchProvider;
        private static TextSearchResult<SearchForThirdPartyInvoicesQueryResult> searchResult;
        private static IEnumerable<SearchForThirdPartyInvoicesQueryResult> results;
        private static SearchForThirdPartyInvoicesQueryResult searchForThirdPartyInvoicesQueryResult;
        private static Dictionary<string, string> expectedFilters;
        private static string filterName = "Filter_Name";
        private static string filterValue = "Filter_Value";
        private static int pageSize;
        private static int currentPage;

        private Establish context = () =>
        {
            pageSize = 20;
            currentPage = 2;
            expectedFilters = new Dictionary<string, string>();
            expectedFilters.Add(filterName, filterValue);
            searchForThirdPartyInvoicesQueryResult = new SearchForThirdPartyInvoicesQueryResult();
            searchForThirdPartyInvoicesQueryResult.ThirdPartyInvoiceKey = 89;
            results = new List<SearchForThirdPartyInvoicesQueryResult>()
             {
                searchForThirdPartyInvoicesQueryResult
             };
            searchResult = new TextSearchResult<SearchForThirdPartyInvoicesQueryResult>(50, 10, 1, 1, results);
            freeTextSearchProvider = An<IFreeTextSearchProvider<SearchForThirdPartyInvoicesQueryResult>>();
            filters = new List<SearchFilter>()
             {
                new SearchFilter(filterName, filterValue)
             };
            query = new SearchForThirdPartyInvoicesQuery("queryText", filters, "indexName");
            freeTextSearchProvider.WhenToldTo(x => x.Search(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Dictionary<string, string>>(), Arg.Any<int>(), Arg.Any<int>()))
                .Return(searchResult);
            handler = new SearchForThirdPartyInvoicesQueryHandler(freeTextSearchProvider);
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