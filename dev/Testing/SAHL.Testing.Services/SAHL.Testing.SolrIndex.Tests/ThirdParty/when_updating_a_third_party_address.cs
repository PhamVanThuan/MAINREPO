using NUnit.Framework;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Testing.Common.Helpers;
using System.Linq;
using SAHL.Testing.SolrIndex.Tests.Extensions;

namespace SAHL.Testing.SolrIndex.Tests.ThirdParty
{
    public class when_updating_a_third_party_address : SolrIndexTest
    {
        private AddressDataModel originalAddress;

        [TearDown]
        public void TearDown()
        {
            if (originalAddress != null)
            {
                var feTestCommand = new UpdateAddressCommand(originalAddress);
                _feTestClient.PerformCommand(feTestCommand, metadata).WithoutMessages();
            }
        }

        [Test]
        public void it_should_update_the_address_field_of_the_third_party_in_the_solr_index()
        {
            //search solr index for third party
            var expectedThirdPartyQuery = new SearchForThirdPartyQuery("* TO *", searchFilters, "ThirdParty");
            _searchService.PerformQuery(expectedThirdPartyQuery).WithoutMessages();
            var expectedThirdParty = expectedThirdPartyQuery.Result.Results.SelectRandom();

            //get legal entity address
            var legalEntityAddress = TestApiClient.GetAny<LegalEntityAddressDataModel>(new { LegalEntityKey = expectedThirdParty.LegalEntityKey });

            //get address
            originalAddress = TestApiClient.GetAny<AddressDataModel>(new { AddressKey = legalEntityAddress.AddressKey });

            //update address
            var updatedAddress = GenericHelpers.DeepCopy(originalAddress);
            updatedAddress.StreetNumber = DataGenerator.RandomInt(1, 100).ToString();
            updatedAddress.StreetName = DataGenerator.RandomString(10);
            var filterAddress = string.Concat(updatedAddress.StreetNumber, " ", updatedAddress.StreetName, " ", originalAddress.RRR_SuburbDescription).Trim();

            //update client address on 2am
            var feTestCommand = new UpdateAddressCommand(updatedAddress);
            _feTestClient.PerformCommand(feTestCommand, metadata).WithoutMessages();

            //search the third party solr index by address and check that the third party is returned in the results
            searchFilters.Clear();
            var actualThirdPartyQuery = new SearchForThirdPartyQuery(filterAddress, searchFilters, "ThirdParty");
            AssertLegalEntityReturnedInThirdPartySearch(actualThirdPartyQuery, expectedThirdParty.LegalEntityKey, 1, 30);
        }
    }
}