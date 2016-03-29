using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Identity;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Testing.Common.Helpers;
using System;
using System.Linq;

namespace SAHL.Testing.SolrIndex.Tests.ThirdParty
{
    public class when_adding_a_valuation_to_an_account : SolrIndexTest
    {
        private Guid valuationId;

        [TearDown]
        public void TearDown()
        {
            if (valuationId != null)
            {
                _linkedKeyManager.DeleteLinkedKey(valuationId);
            }
        }

        [Test]
        public void it_should_associate_the_account_with_the_valuer_in_the_third_party_solr_index()
        {
            //find an open, new purchase account
            var account = TestApiClient.GetAny<AccountDataModel>(new { AccountStatusKey = (int)AccountStatus.Open, ProductKey = (int)Product.NewVariableLoan }, 1000);

            //add a valuation to the offer for a specific valuer
            var valuer = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Valuer, GeneralStatusKey = (int)GeneralStatus.Active });
            var propertyQuery = new GetPropertyForAccountQuery(account.AccountKey);
            _feTestClient.PerformQuery(propertyQuery).WithoutMessages();
            var property = propertyQuery.Result.Results.FirstOrDefault();
            var valuation = new ValuationDataModel(valuer.GenericKey, DateTime.Now, 1000000D, 1000000D, 1000000D, "HaloUser", property.PropertyKey, 0, 1000000D, 0, null, 2, null, (int)ValuationStatus.Pending, "Test", 1, true, (int)HOCRoof.Conventional);
            valuationId = CombGuid.Instance.Generate();
            var insertValautionCommand = new InsertValuationCommand(valuation, valuationId);
            _feTestClient.PerformCommand(insertValautionCommand, metadata);

            //search the third party solr index by accountkey and check that the valuer is returned in the results
            var thirdPartyQuery = new SearchForThirdPartyQuery(account.AccountKey.ToString(), searchFilters, "ThirdParty");
            AssertLegalEntityReturnedInThirdPartySearch(thirdPartyQuery, valuer.LegalEntityKey, 1);
        }
    }
}