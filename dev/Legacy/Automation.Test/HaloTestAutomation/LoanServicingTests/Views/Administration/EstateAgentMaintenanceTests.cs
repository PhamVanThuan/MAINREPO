using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace LoanServicingTests.Views.Administration
{
    [RequiresSTA]
    public class EstateAgentMaintenanceTests : TestBase<LegalEntityOrganisationStructureMaintenanceView>
    {
        private int estateAgentsOrgStructKey;
        private int parentKey;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
            //Remove below
            this.estateAgentsOrgStructKey = base.Service<IControlService>().GetControlNumericValue("EstateAgentChannelRoot");
            this.parentKey = base.Service<IControlService>().GetControlNumericValue("EstateAgentChannelRoot");
        }

        protected override void OnTestFixtureTearDown()
        {
            Service<ILegalEntityOrgStructureService>().DeleteLegalEntityOrganisationStructure("TestEstateAgent_Company");
            Service<ILegalEntityOrgStructureService>().DeleteLegalEntityOrganisationStructure("TestEstateAgent_CompanyUpdate");
            Service<ILegalEntityOrgStructureService>().DeleteLegalEntityOrganisationStructure("TestEstateAgent_CompanyRemove");
            base.OnTestFixtureTearDown();
        }

        [Test]
        public void AddEstateAgentCompany()
        {
            this.Navigate();
            var leos = Service<ILegalEntityOrgStructureService>().GetCompanyLegalEntityOrganisationStructureTestData("TestEstateAgent_Company",
                LegalEntityType.Company, OrganisationType.Company);
            base.View.ClickAdd();
            base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
            base.Browser.Page<MaintenanceLegalEntity>().PopulateAddress(
               new Automation.DataModels.Address()
               {
                   AddressFormatDescription = AddressFormat.Street,
                   AddressTypeDescription = AddressType.Residential,
                   StreetName = "StrN",
                   StreetNumber = "123",
                   RRR_CountryDescription = "South Africa",
                   RRR_ProvinceDescription = "Kwazulu-natal",
                   RRR_SuburbDescription = "Durban North"
               }, DateTime.Now);
            base.Browser.Page<MaintenanceLegalEntity>().Add();
            base.Browser.Page<BasePage>().DomainWarningClickYes();
            MaintenanceViewAssertions.AssertLegalEntityOrganisationStructure(leos.LegalEntity.RegistrationNumber, LegalEntityTypeEnum.Company);
        }

        [Test]
        public void UpdateEstateAgentCompany()
        {
            base.Service<ILegalEntityOrgStructureService>().InsertLegalEntityOrganisationStructure
                    ("TestEstateAgent_CompanyUpdate", LegalEntityTypeEnum.Company, "1234567", this.parentKey);
            var le = base.Service<ILegalEntityService>().GetLegalEntity(registeredname: "TestEstateAgent_CompanyUpdate");
            base.Service<ILegalEntityAddressService>().InsertLegalEntityAddress(new Automation.DataModels.LegalEntityAddress()
               {
                   Address = new Automation.DataModels.Address()
                   {
                       AddressFormatKey = AddressFormatEnum.Street,
                       //AddressFormatDescription = AddressFormat.Street,
                       AddressTypeDescription = AddressType.Residential,
                       StreetName = "StrN",
                       StreetNumber = "123",
                       RRR_CountryDescription = "South Africa",
                       RRR_ProvinceDescription = "Kwazulu-natal",
                       RRR_SuburbDescription = "Durban North"
                   },
                   LegalEntity = le,
                   GeneralStatusKey = GeneralStatusEnum.Active,
                   AddressTypeKey = AddressTypeEnum.Residential
               });

            this.Navigate();
            base.Browser.Page<LegalEntityOrganisationStructureMaintenanceView>().SelectTier("TestEstateAgent_CompanyUpdate");
            var leos = Service<ILegalEntityOrgStructureService>().GetCompanyLegalEntityOrganisationStructureTestData("TestEstateAgent_CompanyUpdate",
                LegalEntityType.Company, OrganisationType.Company);

            //Test

            base.Browser.Page<LegalEntityOrganisationStructureMaintenanceView>().ClickUpdate();
            base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
            base.Browser.Page<MaintenanceLegalEntity>().Update();
            base.Browser.Page<BasePage>().DomainWarningClickYes();
            MaintenanceViewAssertions.AssertLegalEntityOrganisationStructure(leos.LegalEntity.RegistrationNumber, LegalEntityTypeEnum.Company);
        }

        [Test]
        public void DeleteEstateAgentCompany()
        {
            base.Service<ILegalEntityOrgStructureService>().InsertLegalEntityOrganisationStructure
                ("TestEstateAgent_CompanyRemove", LegalEntityTypeEnum.Company, "7654321", this.estateAgentsOrgStructKey);

            this.Navigate();
            var leos = Service<ILegalEntityOrgStructureService>().GetCompanyLegalEntityOrganisationStructureTestData("TestEstateAgent_CompanyRemove",
                LegalEntityType.Company, OrganisationType.Company);
            base.Browser.Page<LegalEntityOrganisationStructureMaintenanceView>().ExpandAll();
            base.Browser.Page<LegalEntityOrganisationStructureMaintenanceView>().SelectTier(leos.LegalEntity.RegisteredName);
            base.Browser.Page<LegalEntityOrganisationStructureMaintenanceView>().ClickRemove();
            base.Browser.Page<MaintenanceLegalEntity>().ClickRemove();
            base.Browser.Page<BasePage>().DomainWarningClickYesHandlingPopUp();
            MaintenanceViewAssertions.AssertLegalEntityOrganisationStructure(leos.LegalEntity.RegistrationNumber, LegalEntityTypeEnum.Company);
        }

        private void Navigate()
        {
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().Menu();
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().AdministrationNode();
            base.Browser.Navigate<AdministrationActions>().EstateAgentMaintenance();
            base.View.ExpandAll();
            base.Browser.Page<LegalEntityOrganisationStructureMaintenanceView>().SelectTier();
        }
    }
}