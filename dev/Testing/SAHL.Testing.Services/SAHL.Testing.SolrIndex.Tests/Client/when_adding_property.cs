using NUnit.Framework;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Testing.Common.Helpers;
using System.Linq;

namespace SAHL.Testing.SolrIndex.Tests.Client
{
    public class when_adding_property : SolrIndexTest
    {
        private OpenNewBusinessApplicationsDataModel application;

        [TearDown]
        public void TearDown()
        {
            if (application != null)
            {
                var command = new RemoveApplicationFromOpenNewBusinessApplicationsCommand(application.OfferKey);
                _feTestClient.PerformCommand(command, metadata);
            }
        }

        [Test]
        public void it_should_add_the_property_details_to_the_client_in_the_solr_index()
        {
            //find an application without a property
            application = TestApiClient.GetAny<OpenNewBusinessApplicationsDataModel>(new { HasProperty = false });

            //find a client linked to the application by OfferKey
            var expectedClientQuery = new SearchForClientQuery(application.OfferKey.ToString(), searchFilters, "Client");
            _searchService.PerformQuery(expectedClientQuery).WithoutMessages();
            var expectedClient = expectedClientQuery.Result.Results.FirstOrDefault();

            //find a property not linked to any offer
            var getPropertyQuery = new GetPropertyNotLinkedToAnApplicationQuery();
            _feTestClient.PerformQuery(getPropertyQuery).WithoutMessages();
            var property = getPropertyQuery.Result.Results.FirstOrDefault();

            //find the address details of the property
            var address = TestApiClient.GetByKey<AddressDataModel>((int)property.AddressKey);
            var filterAddress = string.Concat(address.StreetNumber, " ", address.StreetName, " ", address.RRR_SuburbDescription);

            //link the property to the offer
            var insertPropertyCommand = new LinkOfferMortgageLoanPropertyCommand(application.OfferKey, property.PropertyKey);
            _feTestClient.PerformCommand(insertPropertyCommand, metadata).WithoutMessages();

            //search for the property address in the client index and check that the expected client is returned in the results
            searchFilters.RemoveAll(x => true);
            var actualClientQuery = new SearchForClientQuery(filterAddress, searchFilters, "Client");
            AssertLegalEntityReturnedInClientSearch(actualClientQuery, expectedClient.LegalEntityKey, 1);
        }
    }
}