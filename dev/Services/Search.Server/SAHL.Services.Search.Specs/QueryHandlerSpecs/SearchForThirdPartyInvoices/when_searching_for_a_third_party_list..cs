using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.TextSearch;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Services.Search.QueryHandlers;

namespace SAHL.Services.Search.Specs.QueryHandlerSpecs.SearchForThirdPartyInvoices
{
    public class when_searching_for_a_third_party_list : WithFakes
    {
        private static SearchForThirdPartyInvoicesQuery query;
        private static IEnumerable<SearchFilter> filters;
        private static SearchForThirdPartyInvoicesQueryHandler handler;
        private static IFreeTextSearchProvider<SearchForThirdPartyInvoicesQueryResult> freeTextSearchProvider;
        private static TextSearchResult<SearchForThirdPartyInvoicesQueryResult> searchResult;
        private static IEnumerable<SearchForThirdPartyInvoicesQueryResult> results;
        private static SearchForThirdPartyInvoicesQueryResult queryResult;
        private static Dictionary<string, string> expectedFilters;
        private static string filterName = "Filter_Name";
        private static string filterValue = "Filter_Value";

        private Establish context = () =>
        {
            expectedFilters = new Dictionary<string, string>();
            expectedFilters.Add(filterName, filterValue);
            queryResult = new SearchForThirdPartyInvoicesQueryResult();
            queryResult.ThirdParty = "Randles Inc";
            queryResult.AccountKey = 1323978;
            queryResult.ThirdPartyInvoiceKey = 89;
            queryResult.SahlReference = "SAHL-IntegrationTest89";
            queryResult.InvoiceStatusDescription = "Recieved";
            queryResult.InvoiceNumber = "SAHL-AutomatedTest-1166696025";
            queryResult.InvoiceDate = new DateTime();
            queryResult.ReceivedFromEmailAddress = "clintons@sahomeloans.com";
            queryResult.AmountExcludingVAT = "100";
            queryResult.VATAmount = "10";
            queryResult.TotalAmountIncludingVAT = "110";
            queryResult.CapitaliseInvoice = "1";
            queryResult.ReceivedDate = new DateTime();
            queryResult.SpvDescription = "Tabistone 06 (RF) Limited";
            queryResult.WorkflowProcess = new string[5];
            queryResult.WorkflowStage = new string[5];
            queryResult.AssignedTo = "Unassigned";
            queryResult.InstanceID = "8918000";
            queryResult.GenericKey = "1202";

            results = new List<SearchForThirdPartyInvoicesQueryResult>()
             {
                queryResult
             };
            searchResult = new TextSearchResult<SearchForThirdPartyInvoicesQueryResult>(1, 1, 1, 1, results);
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
            handler.HandleQuery(query);
        };

        private It should_return_the_result_from_the_free_text_search = () =>
        {
            query.Result.Results.First().ShouldEqual(queryResult);
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