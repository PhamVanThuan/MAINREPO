using NUnit.Framework;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Testing.Common.Helpers;
using System.Linq;

namespace SAHL.Testing.SolrIndex.Tests.Client
{
    public class when_updating_client : SolrIndexTest
    {
        private LegalEntityDataModel originalLegalEntity;

        [TearDown]
        public void TearDown()
        {
            if (originalLegalEntity != null)
            {
                var feTestCommand = new UpdateClientCommand(originalLegalEntity);
                _feTestClient.PerformCommand(feTestCommand, metadata).WithoutMessages();
            }
        }

        [Test]
        public void when_successful()
        {
            //Use SearchService to search Client solr Index for a client
            var expectedClientQuery = new SearchForClientQuery("* TO *", searchFilters, "Client");
            _searchService.PerformQuery(expectedClientQuery).WithoutMessages();
            var legalEntityKey = expectedClientQuery.Result.Results.FirstOrDefault().LegalEntityKey;

            //Add a new UpdateClientCommand to FrentEndTestService that updates the LegalEntity record
            originalLegalEntity = TestApiClient.GetByKey<LegalEntityDataModel>(legalEntityKey);
            var updatedLegalEntity = GenericHelpers.DeepCopy(originalLegalEntity);
            updatedLegalEntity.EmailAddress = DataGenerator.RandomEmailAddress();
            var UpdateCommand = new UpdateClientCommand(updatedLegalEntity);
            _feTestClient.PerformCommand(UpdateCommand, metadata).WithoutMessages();

            //Use SearchService to search Client Solr Index for the client
            //and check that the LegalEntity updates have been made on the index
            var actualClientQuery = new SearchForClientQuery(updatedLegalEntity.EmailAddress, searchFilters, "Client");
            AssertLegalEntityReturnedInClientSearch(actualClientQuery, originalLegalEntity.LegalEntityKey, 1);
        }
    }
}