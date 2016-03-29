using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Testing.Common.Helpers;
using System.Linq;
using SAHL.Testing.SolrIndex.Tests.Extensions;

namespace SAHL.Testing.SolrIndex.Tests.ThirdParty
{
    public class when_adding_a_third_party_address : SolrIndexTest
    {
        [Test]
        public void it_should_update_the_address_of_the_third_party_in_the_solr_index()
        {
            //find a third party in the third party solr index
            var expectedThirdPartyQuery = new SearchForThirdPartyQuery("* TO *", searchFilters, "ThirdParty");
            _searchService.PerformQuery(expectedThirdPartyQuery).WithoutMessages();
            var expectedThirdParty = expectedThirdPartyQuery.Result.Results.SelectRandom();

            //find an address
            var address = TestApiClient.GetAny<AddressDataModel>(new { AddressFormatKey = (int)AddressFormat.Street }, 1000);
            var filterAddress = string.Concat(address.StreetNumber, " ", address.StreetName, " ", address.RRR_SuburbDescription);

            //link an address to the third party legalentity
            var feTestCommand = new LinkLegalEntityAddressCommand(expectedThirdParty.LegalEntityKey, address.AddressKey, AddressType.Residential);
            _feTestClient.PerformCommand(feTestCommand, metadata).WithoutMessages();

            //search the third party solr index by address and check that the third party is returned in the results
            var actualThirdPartyQuery = new SearchForThirdPartyQuery(filterAddress, searchFilters, "ThirdParty");
            AssertLegalEntityReturnedInThirdPartySearch(actualThirdPartyQuery, expectedThirdParty.LegalEntityKey, 1);
        }
    }
}