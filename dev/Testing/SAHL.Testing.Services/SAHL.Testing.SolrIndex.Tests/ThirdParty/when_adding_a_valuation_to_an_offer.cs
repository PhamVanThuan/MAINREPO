using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Identity;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.Search.Queries;
using System;
using System.Linq;

namespace SAHL.Testing.SolrIndex.Tests.ThirdParty
{
    public class when_adding_a_valuation_to_an_offer : SolrIndexTest
    {
        private Guid valuationId;
        private OpenNewBusinessApplicationsDataModel application;

        [TearDown]
        public void TearDown()
        {
            if (valuationId != null)
            {
                _linkedKeyManager.DeleteLinkedKey(valuationId);
                var removeApplication = new RemoveApplicationFromOpenNewBusinessApplicationsCommand(application.OfferKey);
                _feTestClient.PerformCommand(removeApplication, metadata);
            }
        }

        [Test]
        public void it_should_associate_the_offer_with_the_valuer_in_the_third_party_solr_index()
        {
            //find an open, new purchase offer
            application = TestApiClient.GetAny<OpenNewBusinessApplicationsDataModel>(new { HasProperty = true });

            //add a valuation to the offer for a specific valuer
            var valuer = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Valuer, GeneralStatusKey = (int)GeneralStatus.Active });
            var valuation = new ValuationDataModel(valuer.GenericKey, DateTime.Now, 1000000D, 1000000D, 1000000D, "HaloUser", (int)application.PropertyKey, 0, 1000000D, 0, null, 2, null, (int)ValuationStatus.Pending, "Test", 1, true, (int)HOCRoof.Conventional);
            valuationId = CombGuid.Instance.Generate();
            var insertValautionCommand = new InsertValuationCommand(valuation, valuationId);
            _feTestClient.PerformCommand(insertValautionCommand, metadata);

            //search the third party solr index by offerkey and check that the valuer is returned in the results
            var thirdPartyQuery = new SearchForThirdPartyQuery(application.OfferKey.ToString(), searchFilters, "ThirdParty");
            AssertLegalEntityReturnedInThirdPartySearch(thirdPartyQuery, valuer.LegalEntityKey, 1);
        }
    }
}