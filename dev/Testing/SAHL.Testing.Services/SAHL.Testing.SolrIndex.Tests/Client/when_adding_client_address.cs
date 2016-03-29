using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Testing.Common.Helpers;
using System.Linq;

namespace SAHL.Testing.SolrIndex.Tests.Client
{
    public class when_adding_client_address : SolrIndexTest
    {
        [Test]
        public void it_should_update_the_legal_entity_address_field_of_the_client_in_the_solr_index()
        {
            //find existing address
            var address = TestApiClient.GetAny<AddressDataModel>(new { AddressFormatKey = (int)AddressFormat.Street }, 1000);
            var filterAddress = string.Concat(address.StreetNumber, " ", address.StreetName, " ", address.RRR_SuburbDescription).Trim();

            //search solr index for client with blank property address field
            searchFilters.Add(new SearchFilter("LegalEntityAddress", "\"\" TO *"));
            var expectedClientQuery = new SearchForClientQuery("Open", searchFilters, "Client");
            _searchService.PerformQuery(expectedClientQuery).WithoutMessages();
            var expectedClient = expectedClientQuery.Result.Results.FirstOrDefault();

            //link address to client on 2am
            var feTestCommand = new LinkLegalEntityAddressCommand(expectedClient.LegalEntityKey, address.AddressKey, AddressType.Residential);
            _feTestClient.PerformCommand(feTestCommand, metadata).WithoutMessages();

            //search solr index filtered by linked address
            searchFilters.RemoveAll(x => true);
            var actualClientQuery = new SearchForClientQuery(filterAddress, searchFilters, "Client");
            AssertLegalEntityReturnedInClientSearch(actualClientQuery, expectedClient.LegalEntityKey, 1, 30);
        }
    }
}