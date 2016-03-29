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
    public class when_adding_a_foreclosure_detail : SolrIndexTest
    {
        private Guid loanDetailId;

        [TearDown]
        public void TearDown()
        {
            if (loanDetailId != null)
            {
                _linkedKeyManager.DeleteLinkedKey(loanDetailId);
            }
        }

        [Test]
        public void it_should_associate_the_account_with_the_attorney_in_the_third_party_solr_index()
        {
            //add a foreclosure detail
            var account = TestApiClient.GetAny<AccountDataModel>(new { AccountStatusKey = (int)AccountStatus.Open, ProductKey = (int)Product.NewVariableLoan }, 1000);
            var foreclosureAttorneyDetailType = TestApiClient.GetAny<ForeclosureAttorneyDetailTypeMappingDataModel>(new { GeneralStatusKey = (int)GeneralStatus.Active });
            var loanDetail = new DetailDataModel((int)foreclosureAttorneyDetailType.DetailTypeKey, account.AccountKey, DateTime.Now, null, string.Empty, null, @"SAHL\HaloUser", null);
            loanDetailId = CombGuid.Instance.Generate();
            var insertLoanDetailCommand = new InsertLoanDetailCommand(loanDetail, loanDetailId);
            _feTestClient.PerformCommand(insertLoanDetailCommand, metadata).WithoutMessages();

            //search third party solr index by AccountKey and check that the foreclosure attorney is returned in the results
            var thirdPartyQuery = new SearchForThirdPartyQuery(account.AccountKey.ToString(), searchFilters, "ThirdParty");
            AssertLegalEntityReturnedInThirdPartySearch(thirdPartyQuery, Convert.ToInt32(foreclosureAttorneyDetailType.LegalEntityKey), 1);
        }
    }
}