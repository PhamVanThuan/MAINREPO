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
    public class when_updating_an_attorney : SolrIndexTest
    {
        private AttorneyDataModel originalAttorney;

        [TearDown]
        public void TearDown()
        {
            if (originalAttorney != null)
            {
                var updateValuerCommand = new UpdateAttorneyCommand(originalAttorney);
                _feTestClient.PerformCommand(updateValuerCommand, metadata);
            }
        }

        [Test]
        public void it_should_update_the_attorney_details_in_the_solr_index()
        {
            searchFilters.Add(new SearchFilter("ThirdPartyType", "Attorney"));
            var expectedThirdPartyQuery = new SearchForThirdPartyQuery("* TO *", searchFilters, "ThirdParty");
            _searchService.PerformQuery(expectedThirdPartyQuery).WithoutMessages();
            var expectedThirdParty = expectedThirdPartyQuery.Result.Results.SelectRandom();

            originalAttorney = TestApiClient.GetAny<AttorneyDataModel>(new { LegalEntityKey = expectedThirdParty.LegalEntityKey });

            var updatedAttorney = GenericHelpers.DeepCopy(originalAttorney);
            updatedAttorney.AttorneyContact = DataGenerator.RandomString(10);

            var updateAttorneyCommand = new UpdateAttorneyCommand(updatedAttorney);
            _feTestClient.PerformCommand(updateAttorneyCommand, metadata).WithoutMessages();

            searchFilters.RemoveAll(x => true);
            var thirdPartyQuery = new SearchForThirdPartyQuery(updatedAttorney.AttorneyContact, searchFilters, "ThirdParty");
            AssertLegalEntityReturnedInThirdPartySearch(thirdPartyQuery, originalAttorney.LegalEntityKey, 1);
        }
    }
}