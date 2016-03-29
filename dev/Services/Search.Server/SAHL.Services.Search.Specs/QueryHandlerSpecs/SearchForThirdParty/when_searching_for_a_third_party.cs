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

namespace SAHL.Services.Search.Specs.QueryHandlerSpecs.SearchForThirdParty
{
    public class when_searching_for_a_third_party : WithFakes
    {
        private static SearchForThirdPartyQuery query;
        private static IEnumerable<SearchFilter> filters;
        private static SearchForThirdPartyQueryHandler handler;
        private static IFreeTextSearchProvider<SearchForThirdPartyQueryResult> freeTextSearchProvider;
        private static TextSearchResult<SearchForThirdPartyQueryResult> searchResult;
        private static IEnumerable<SearchForThirdPartyQueryResult> results;
        private static SearchForThirdPartyQueryResult searchForClientQueryResult;
        private static Dictionary<string, string> expectedFilters;
        private static string filterName = "Filter_Name";
        private static string filterValue = "Filter_Value";

        private Establish context = () =>
        {
            expectedFilters = new Dictionary<string, string>();
            expectedFilters.Add(filterName, filterValue);
            searchForClientQueryResult = new SearchForThirdPartyQueryResult();
            searchForClientQueryResult.LegalEntityKey = 1;
            searchForClientQueryResult.LegalName = "Bob Speed";
            searchForClientQueryResult.CellPhoneNumber = "0714033283";
            results = new List<SearchForThirdPartyQueryResult>()
             {
                searchForClientQueryResult
             };
            searchResult = new TextSearchResult<SearchForThirdPartyQueryResult>(1, 1, 1, 1, results);
            freeTextSearchProvider = An<IFreeTextSearchProvider<SearchForThirdPartyQueryResult>>();
            filters = new List<SearchFilter>()
             {
                new SearchFilter(filterName, filterValue)
             };
            query = new SearchForThirdPartyQuery("queryText", filters, "indexName");
            freeTextSearchProvider.WhenToldTo(x => x.Search(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Dictionary<string, string>>(), Arg.Any<int>(), Arg.Any<int>()))
            .Return(searchResult);
            handler = new SearchForThirdPartyQueryHandler(freeTextSearchProvider);
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
