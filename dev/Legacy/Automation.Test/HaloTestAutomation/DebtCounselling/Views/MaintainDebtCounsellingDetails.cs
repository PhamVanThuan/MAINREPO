using BuildingBlocks;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using Navigation = BuildingBlocks.Navigation;

namespace DebtCounsellingTests.Views
{
    [Ignore]
    [TestFixture, RequiresSTA]
    internal class MaintainDebtCounsellingDetails : TestBase<LegalEntityOrganisationStructureMaintenanceView>
    {
        private int debtCounsellorOrgStructKey;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingManager);
            this.debtCounsellorOrgStructKey = base.Service<IControlService>().GetControlNumericValue("DebtCounsellorRoot");
        }

        [Test]
        [Ignore]
        public void AddDebtCounsellorCompany()
        {
            try
            {
                this.SelectDebtCounsellor_RootNode();
                var leos = base.Service<ILegalEntityOrgStructureService>().GetCompanyLegalEntityOrganisationStructureTestData("TestDebtCounsellor_Company",
                    LegalEntityType.Company, OrganisationType.Company);
                base.Browser.Page<LegalEntityOrganisationStructureMaintenanceView>().ClickAdd();
                base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
                base.Browser.Page<MaintenanceLegalEntity>().Add();

                //Assert

                //Cleanup
                base.Service<ILegalEntityOrgStructureService>().DeleteLegalEntityOrganisationStructure("TestDebtCounsellor_Company");
            }
            catch
            {
                throw;
            }
        }

        [Test]
        [Ignore]
        public void UpdateDebtCounsellor()
        {
            try
            {
                Service<ILegalEntityOrgStructureService>().InsertLegalEntityOrganisationStructure
                    ("TestDebtCounsellor_CompanyUpdate", LegalEntityTypeEnum.Company, "1234567", this.debtCounsellorOrgStructKey);

                this.SelectDebtCounsellor_RootNode();
                base.Browser.Page<LegalEntityOrganisationStructureMaintenanceView>().SelectTier("TestDebtCounsellor_CompanyUpdate");
                var leos = Service<ILegalEntityOrgStructureService>().GetCompanyLegalEntityOrganisationStructureTestData("TestDebtCounsellor_CompanyUpdate",
                    LegalEntityType.Company, OrganisationType.Company);

                //Test
                base.Browser.Page<LegalEntityOrganisationStructureMaintenanceView>().ClickUpdate();
                base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
                base.Browser.Page<MaintenanceLegalEntity>().Update();

                //Assert

                //Cleanup
                base.Service<ILegalEntityOrgStructureService>().DeleteLegalEntityOrganisationStructure("TestDebtCounsellor_CompanyUpdate");
            }
            catch
            {
                throw;
            }
        }

        [Test]
        [Ignore]
        public void DeleteDebtCounsellor()
        {
            try
            {
                Service<ILegalEntityOrgStructureService>().InsertLegalEntityOrganisationStructure
                    ("TestDebtCounsellor_CompanyRemove", LegalEntityTypeEnum.Company, "7654321", this.debtCounsellorOrgStructKey);
                this.SelectDebtCounsellor_RootNode();
                var leos = Service<ILegalEntityOrgStructureService>().GetCompanyLegalEntityOrganisationStructureTestData("TestDebtCounsellor_CompanyRemove",
                    LegalEntityType.Company, OrganisationType.Company);
                base.Browser.Page<LegalEntityOrganisationStructureMaintenanceView>().ClickRemove();
                base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
                base.Browser.Page<MaintenanceLegalEntity>().ClickRemove();
            }
            catch
            {
                throw;
            }
        }

        #region Helpers

        private void SelectDebtCounsellor_RootNode()
        {
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<Navigation.MenuNode>().LossControlNode();
            base.Browser.Navigate<Navigation.MenuNode>().DebtCounsellorMaintenance();
            base.Browser.Page<LegalEntityOrganisationStructureMaintenanceView>().SelectTier();
        }

        #endregion Helpers
    }
}