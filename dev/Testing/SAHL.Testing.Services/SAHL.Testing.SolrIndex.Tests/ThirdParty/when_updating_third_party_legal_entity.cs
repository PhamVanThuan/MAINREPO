using NUnit.Framework;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Testing.Common.Helpers;
using System.Linq;
using SAHL.Testing.SolrIndex.Tests.Extensions;

namespace SAHL.Testing.SolrIndex.Tests.ThirdParty
{
    [TestFixture]
    public class when_updating_third_party_legal_entity : SolrIndexTest
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
            var expectedThirdPartyQuery = new SearchForThirdPartyQuery("* TO *", searchFilters, "ThirdParty");
            _searchService.PerformQuery(expectedThirdPartyQuery).WithoutMessages();
            var legalEntityKey = expectedThirdPartyQuery.Result.Results.SelectRandom().LegalEntityKey;

            originalLegalEntity = TestApiClient.GetByKey<LegalEntityDataModel>(legalEntityKey);
            var updatedLegalEntity = GenericHelpers.DeepCopy(originalLegalEntity);
            updatedLegalEntity.EmailAddress = DataGenerator.RandomEmailAddress();
            var UpdateCommand = new UpdateThirdPartyCommand(updatedLegalEntity);
            _feTestClient.PerformCommand(UpdateCommand, metadata).WithoutMessages();

            var actualThirdPartyQuery = new SearchForThirdPartyQuery(updatedLegalEntity.EmailAddress, searchFilters, "ThirdParty");
            AssertLegalEntityReturnedInThirdPartySearch(actualThirdPartyQuery, legalEntityKey, 1);
        }
    }
}