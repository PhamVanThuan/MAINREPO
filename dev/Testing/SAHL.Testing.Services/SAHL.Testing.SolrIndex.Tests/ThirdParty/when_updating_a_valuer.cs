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
    public class when_updating_a_valuer : SolrIndexTest
    {
        private ValuatorDataModel originalValuer;

        [TearDown]
        public void TearDown()
        {
            if (originalValuer != null)
            {
                var updateValuerCommand = new UpdateValuatorCommand(originalValuer);
                _feTestClient.PerformCommand(updateValuerCommand, metadata);
            }
        }

        [Test]
        public void it_should_update_the_valuer_details_in_the_solr_index()
        {
            searchFilters.Add(new SearchFilter("ThirdPartyType", "Valuer"));
            var expectedThirdPartyQuery = new SearchForThirdPartyQuery("* TO *", searchFilters, "ThirdParty");
            _searchService.PerformQuery(expectedThirdPartyQuery).WithoutMessages();
            var expectedThirdParty = expectedThirdPartyQuery.Result.Results.SelectRandom();

            originalValuer = TestApiClient.GetAny<ValuatorDataModel>(new { LegalEntityKey = expectedThirdParty.LegalEntityKey });

            var updatedValuer = GenericHelpers.DeepCopy(originalValuer);
            updatedValuer.ValuatorContact = DataGenerator.RandomString(10);

            var updateValuerCommand = new UpdateValuatorCommand(updatedValuer);
            _feTestClient.PerformCommand(updateValuerCommand, metadata).WithoutMessages();

            searchFilters.RemoveAll(x => true);
            var thirdPartyQuery = new SearchForThirdPartyQuery(updatedValuer.ValuatorContact, searchFilters, "ThirdParty");
            AssertLegalEntityReturnedInThirdPartySearch(thirdPartyQuery, originalValuer.LegalEntityKey, 1, 60);
        }
    }
}