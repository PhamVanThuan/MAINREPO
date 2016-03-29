using NUnit.Framework;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Testing.Common.Helpers;
using System.Linq;

namespace SAHL.Testing.SolrIndex.Tests.Client
{
    public class when_updating_property : SolrIndexTest
    {
        private AddressDataModel originalAddress;

        [TearDown]
        public void TearDown()
        {
            if (originalAddress != null)
            {
                var updateAddresCommand = new UpdateAddressCommand(originalAddress);
                _feTestClient.PerformCommand(updateAddresCommand, metadata);
            }
        }

        [Test]
        public void it_should()
        {
            //find an application without a property
            var application = TestApiClient.GetAny<OpenNewBusinessApplicationsDataModel>(new { HasProperty = true });

            //find a client linked to the application by OfferKey
            var expectedClientQuery = new SearchForClientQuery(application.OfferKey.ToString(), searchFilters, "Client");
            _searchService.PerformQuery(expectedClientQuery).WithoutMessages();
            var expectedClient = expectedClientQuery.Result.Results.FirstOrDefault();

            //update the property address
            var offerMortgageLoan = TestApiClient.GetByKey<OfferMortgageLoanDataModel>(application.OfferKey);
            var property = TestApiClient.GetByKey<PropertyDataModel>((int)offerMortgageLoan.PropertyKey);
            originalAddress = TestApiClient.GetByKey<AddressDataModel>((int)property.AddressKey);
            var updatedAddress = GenericHelpers.DeepCopy(originalAddress);
            updatedAddress.StreetNumber = DataGenerator.RandomInt(0, 999).ToString();
            updatedAddress.StreetName = DataGenerator.RandomString(10);
            var filterAddress = string.Concat(updatedAddress.StreetNumber, " ", updatedAddress.StreetName, " ", originalAddress.RRR_SuburbDescription);
            var updateAddressCommand = new UpdateAddressCommand(updatedAddress);
            _feTestClient.PerformCommand(updateAddressCommand, metadata).WithoutMessages();

            //search for the property address in the client index and check that the expected client is returned in the results
            searchFilters.RemoveAll(x => true);
            var actualClientQuery = new SearchForClientQuery(filterAddress, searchFilters, "Client");
            AssertLegalEntityReturnedInClientSearch(actualClientQuery, expectedClient.LegalEntityKey, 1);
        }
    }
}