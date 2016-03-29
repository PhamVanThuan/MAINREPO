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
    public class when_adding_a_valuer : SolrIndexTest
    {
        private Guid valuerId;

        [TearDown]
        public void TearDown()
        {
            if (valuerId != null)
            {
                var valuerKey = _linkedKeyManager.RetrieveLinkedKey(valuerId);
                var removeValuerCommand = new RemoveValuerCommand(valuerKey);
                _feTestClient.PerformCommand(removeValuerCommand, metadata);
                _linkedKeyManager.DeleteLinkedKey(valuerId);
            }
        }

        [Test]
        public void it_should_add_the_valuer_to_the_third_party_solr_index()
        {
            //add a valuer
            var company = TestApiClient.GetAny<LegalEntityDataModel>(new { LegalEntityTypeKey = (int)LegalEntityType.Company }, 1000);
            var valuer = new ValuatorDataModel("The Valuator", string.Empty, 0, (int)GeneralStatus.Active, company.LegalEntityKey);
            valuerId = CombGuid.Instance.Generate();
            var insertValuerCommand = new InsertValuerCommand(valuer, valuerId);
            _feTestClient.PerformCommand(insertValuerCommand, metadata).WithoutMessages();

            //search third party solr index for valuer and check that it is returned in the results
            var thirdPartyQuery = new SearchForThirdPartyQuery(valuer.LegalEntityKey.ToString(), searchFilters, "ThirdParty");
            AssertLegalEntityReturnedInThirdPartySearch(thirdPartyQuery, valuer.LegalEntityKey, 1);
        }
    }
}