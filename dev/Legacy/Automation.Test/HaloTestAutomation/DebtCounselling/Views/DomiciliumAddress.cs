using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace DebtCounsellingTests.Views
{
    [TestFixture, RequiresSTA]
    public sealed class DomiciliumAddressTests : DebtCounsellingTests.TestBase<LegalEntityDomiciliumAddress>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        [Test]
        public void AddActiveDomiciliumForLegalEntityUnderDebtCounselling()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            //get first legal entity
            var legalEntity = Service<IExternalRoleService>().GetFirstActiveExternalRole(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                ExternalRoleTypeEnum.Client, true);
            //remove domiciliums
            Service<ILegalEntityAddressService>().DeleteLegalEntityDomiciliumAddress(legalEntity.LegalEntityKey);
            var leAddress = Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(legalEntity.LegalEntityKey, AddressFormatEnum.Street, AddressTypeEnum.Residential, GeneralStatusEnum.Active);
            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntity.LegalEntityKey);
            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().DomiciliuAddressDetails(NodeTypeEnum.Update);
            base.View.SelectDomiciliumAddress(leAddress.DelimitedAddress);
            base.View.ClickSubmit();
            LegalEntityAssertions.AssertLegalEntityDomicilium(leAddress, GeneralStatusEnum.Active);
        }
    }
}