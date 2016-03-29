using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Identity;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Testing.Common.Helpers;
using System;

namespace SAHL.Testing.SolrIndex.Tests.ThirdParty
{
    public class when_adding_an_attorney : SolrIndexTest
    {
        private Guid attorneyId;

        [TearDown]
        public void TearDown()
        {
            if (attorneyId != null)
            {
                var attorneyKey = _linkedKeyManager.RetrieveLinkedKey(attorneyId);
                var removeAttorneyCommand = new RemoveAttorneyCommand(attorneyKey);
                _feTestClient.PerformCommand(removeAttorneyCommand, metadata);
                _linkedKeyManager.DeleteLinkedKey(attorneyId);
            }
        }

        [Test]
        public void it_should_add_the_attorney_to_the_third_party_solr_index()
        {
            //add a valuer
            var company = TestApiClient.GetAny<LegalEntityDataModel>(new { LegalEntityTypeKey = (int)LegalEntityType.Company }, 1000);
            var attorney = new AttorneyDataModel(6, "Harvey Dent", 1700000D, 1, 55000000D, 0, true, company.LegalEntityKey, true, (int)GeneralStatus.Active);
            attorneyId = CombGuid.Instance.Generate();
            var insertAttorneyCommand = new InsertAttorneyCommand(attorney, attorneyId);
            _feTestClient.PerformCommand(insertAttorneyCommand, metadata).WithoutMessages();

            //search third party solr index for valuer and check that it is returned in the results
            var thirdPartyQuery = new SearchForThirdPartyQuery(attorney.LegalEntityKey.ToString(), searchFilters, "ThirdParty");
            AssertLegalEntityReturnedInThirdPartySearch(thirdPartyQuery, attorney.LegalEntityKey, 1);
        }
    }
}