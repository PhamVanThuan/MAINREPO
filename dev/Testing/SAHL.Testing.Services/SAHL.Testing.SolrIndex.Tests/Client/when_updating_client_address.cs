using NUnit.Framework;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Testing.Common.Helpers;
using System.Linq;

namespace SAHL.Testing.SolrIndex.Tests.Client
{
    public class when_updating_client_address : SolrIndexTest
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
        public void it_should_update_the_legal_entity_address_field_of_the_client_in_the_solr_index()
        {
            //search solr index for client with a populated property address field
            searchFilters.Add(new SearchFilter("LegalEntityAddress", "* TO *"));
            var expectedClientQuery = new SearchForClientQuery("Open", searchFilters, "Client");
            _searchService.PerformQuery(expectedClientQuery).WithoutMessages();
            var expectedClient = expectedClientQuery.Result.Results.FirstOrDefault();

            //get legal entity address
            var legalEntityAddress = TestApiClient.GetAny<LegalEntityAddressDataModel>(new { LegalEntityKey = expectedClient.LegalEntityKey });

            //get address
            originalAddress = TestApiClient.GetAny<AddressDataModel>(new { AddressKey = legalEntityAddress.AddressKey });

            //update address
            var updatedAddress = GenericHelpers.DeepCopy(originalAddress);
            updatedAddress.StreetNumber = DataGenerator.RandomInt(1, 100).ToString();
            updatedAddress.StreetName = DataGenerator.RandomString(10);
            var filterAddress = string.Concat(updatedAddress.StreetNumber, " ", updatedAddress.StreetName, " ", originalAddress.RRR_SuburbDescription);

            //update client address on 2am
            var feTestCommand = new UpdateAddressCommand(updatedAddress);
            _feTestClient.PerformCommand(feTestCommand, metadata).WithoutMessages();

            //search solr index filtered by updated address
            searchFilters.RemoveAll(x => true);
            var actualClientQuery = new SearchForClientQuery(filterAddress, searchFilters, "Client");
            AssertLegalEntityReturnedInClientSearch(actualClientQuery, expectedClient.LegalEntityKey, 1);
        }
    }
}