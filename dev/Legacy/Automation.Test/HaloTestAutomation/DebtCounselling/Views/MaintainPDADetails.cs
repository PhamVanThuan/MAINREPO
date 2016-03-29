using System;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

using Navigation = BuildingBlocks.Navigation;

namespace DebtCounsellingTests.Views
{
    //[Ignore] //Ticket #19411
    [TestFixture, RequiresSTA]
    public class MaintainPDADetailsTests : DebtCounsellingTests.TestBase<LegalEntityOrganisationStructureMaintenanceView>
    {
        private Random random;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingAdmin, TestUsers.Password);
            random = new Random();
            Service<ILegalEntityOrgStructureService>().AddAndMaintainPDATestDataInOrganisationStructure();
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<Navigation.MenuNode>().LossControlNode(); ;
            base.Browser.Navigate<Navigation.MenuNode>().DebtCounselling();
            base.Browser.Navigate<Navigation.MenuNode>().PDAMaintenance();
            base.View.ExpandAll();
        }

        [Test, Description("")]
        public void _001_AddCompany()
        {
            base.View.ClickAdd();
            var leos = Service<ILegalEntityOrgStructureService>().GetCompanyLegalEntityOrganisationStructureTestData("CompanyAdded", "Company", "Company");
            var address = Service<IAddressService>().GetStreetAddressData();
            base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
            base.Browser.Page<MaintenanceLegalEntity>().PopulateAddress(address, AddressType.Residential, AddressFormat.Street, DateTime.Now);
            base.Browser.Page<MaintenanceLegalEntity>().Add();
            base.View.AssertDetailsAdded("Payment Distribution Agencies", "CompanyAdded");
        }

        [Test, Description("")]
        public void _002_AddBranch()
        {
            var leos = Service<ILegalEntityOrgStructureService>().GetCompanyLegalEntityOrganisationStructureTestData("BranchAdded", "Company", "Branch");
            var address = Service<IAddressService>().GetStreetAddressData();

            base.View.SelectTier("CompanyToAddTo");
            base.View.ClickAdd();
            base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
            base.Browser.Page<MaintenanceLegalEntity>().PopulateAddress(address, AddressType.Residential, AddressFormat.Street, DateTime.Now);
            base.Browser.Page<MaintenanceLegalEntity>().Add();
            base.View.AssertDetailsAdded("Payment Distribution Agencies", "CompanyToAddTo", "BranchAdded");
        }

        [Test, Description("")]
        public void _003_AddDepartment()
        {
            var leos = Service<ILegalEntityOrgStructureService>().GetCompanyLegalEntityOrganisationStructureTestData("DepartmentAdded", "Company", "Department");

            base.View.SelectTier("CompanyToAddTo", "BranchToAddTo");
            base.View.ClickAdd();
            base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
            base.Browser.Page<MaintenanceLegalEntity>().Add();
            base.View.AssertDetailsAdded("Payment Distribution Agencies", "CompanyToAddTo", "BranchToAddTo", "DepartmentAdded");
        }

        [Test, Description("")]
        public void _004_AddContact()
        {
            var leos = Service<ILegalEntityOrgStructureService>().GetNaturalLegalEntityOrganisationStructureAddTestData();

            base.View.SelectTier("CompanyToAddTo", "BranchToAddTo", "DepartmentToAddTo");
            base.View.ClickAdd();
            base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
            base.Browser.Page<MaintenanceLegalEntity>().Add();
            base.View.AssertDetailsAdded("Payment Distribution Agencies", "CompanyToAddTo", "BranchToAddTo", "DepartmentToAddTo", "Mr Contact Added");
        }

        [Test, Description("")]
        public void _005_UpdateContact()
        {
            string companyName = "CompanyToUpdate";
            string branchName = "BranchToUpdate";
            string departmentName = "DepartmentToUpdate";

            base.View.SelectTier(companyName, branchName, departmentName, "Mr Contact To Update");
            base.View.ClickUpdate();

            var leos = Service<ILegalEntityOrgStructureService>().GetNaturalLegalEntityOrganisationStructureUpdateTestData();

            base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
            base.Browser.Page<MaintenanceLegalEntity>().Update();

            //Need to view the legalentity else the assertion will fail
            base.View.ClickView();

            MaintenanceViewAssertions.AssertLegalEntityOrganisationStructure(base.Browser, leos);
        }

        [Test, Description("")]
        public void _006_UpdateDepartment()
        {
            string companyName = "CompanyToUpdate";
            string branchName = "BranchToUpdate";
            string departmentName = "DepartmentUpdated";

            var leos = Service<ILegalEntityOrgStructureService>().GetCompanyLegalEntityOrganisationStructureTestData(departmentName, "Company", "Department");

            base.View.SelectTier(companyName, branchName, "DepartmentToUpdate");
            base.View.ClickUpdate();

            base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
            base.Browser.Page<MaintenanceLegalEntity>().Update();

            //Need to view the legalentity else the assertion will fail
            base.View.ClickView();

            MaintenanceViewAssertions.AssertLegalEntityOrganisationStructure(base.Browser, leos);
        }

        [Test, Description("")]
        public void _007_UpdateBranch()
        {
            string companyName = "CompanyToUpdate";
            string branchName = "BranchUpdated";

            base.View.SelectTier(companyName, "BranchToUpdate");
            base.View.ClickUpdate();

            var leos = Service<ILegalEntityOrgStructureService>().GetCompanyLegalEntityOrganisationStructureTestData(branchName, "Company", "Branch");

            base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
            base.Browser.Page<MaintenanceLegalEntity>().Update();

            //Need to view the legalentity else the assertion will fail
            base.View.ClickView();

            MaintenanceViewAssertions.AssertLegalEntityOrganisationStructure(base.Browser, leos);
        }

        [Test, Description("")]
        public void _008_UpdateCompany()
        {
            string companyName = "CompanyUpdated";

            base.View.SelectTier("CompanyToUpdate");
            base.View.ClickUpdate();

            var leos = Service<ILegalEntityOrgStructureService>().GetCompanyLegalEntityOrganisationStructureTestData(companyName, "Company", "Company");

            base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
            base.Browser.Page<MaintenanceLegalEntity>().Update();

            //Need to view the legalentity else the assertion will fail
            base.View.ClickView();

            MaintenanceViewAssertions.AssertLegalEntityOrganisationStructure(base.Browser, leos);
        }

        [Test, Description("")]
        public void _009_RemoveContact()
        {
            base.View.SelectTier("CompanyToRemove", "BranchToRemove", "DepartmentToRemove", "Mr Contact To Remove");
            base.View.ClickRemove();
            base.Browser.Page<MaintenanceLegalEntity>().ClickRemove();
            base.View.AssertDetailsRemoved("Payment Distribution Agencies", "CompanyToRemove", "BranchToRemove", "DepartmentToRemove", "Mr Contact To Remove");
        }

        [Test, Description("")]
        public void _010_RemoveDepartment()
        {
            base.View.SelectTier("CompanyToRemove", "BranchToRemove", "DepartmentToRemove");
            base.View.ClickRemove();
            base.Browser.Page<MaintenanceLegalEntity>().ClickRemove();

            base.View.AssertDetailsRemoved("Payment Distribution Agencies", "CompanyToRemove", "BranchToRemove", "DepartmentToRemove");
        }

        [Test, Description("")]
        public void _011_RemoveBranch()
        {
            base.View.SelectTier("CompanyToRemove", "BranchToRemove");
            base.View.ClickRemove();
            base.Browser.Page<MaintenanceLegalEntity>().ClickRemove();
            base.View.AssertDetailsRemoved("Payment Distribution Agencies", "CompanyToRemove", "BranchToRemove");
        }

        [Test, Description("")]
        public void _012_RemoveCompany()
        {
            base.View.SelectTier("CompanyToRemove");
            base.Browser.Page<MaintenanceLegalEntity>().ClickRemove();
            base.View.AssertDetailsRemoved("Payment Distribution Agencies", "CompanyToRemove");
        }

        [Test, Description("")]
        public void _013_AddCompanyMandatoryOptionalFieldValidation()
        {
            base.View.SelectTier();
            base.View.ClickAdd();

            CompanyMandatoryOptionalFieldValidation(false);
        }

        [Test, Description("")]
        public void _014_AddBranchMandatoryOptionalFieldValidation()
        {
            base.View.SelectTier("CompanyToAddTo");
            base.View.ClickAdd();

            BranchMandatoryOptionalFieldValidation(false);
        }

        [Test, Description("")]
        public void _015_AddDepartmentMandatoryOptionalFieldValidation()
        {
            base.View.SelectTier("CompanyToAddTo", "BranchToAddTo");

            base.View.ClickAdd();
            DepartmentMandatoryOptionalFieldValidation(false);
        }

        [Test, Description("")]
        public void _016_AddContactMandatoryOptionalFieldValidation()
        {
            base.View.SelectTier("CompanyToAddTo", "BranchToAddTo", "DepartmentToAddTo");
            base.View.ClickAdd();

            ContactMandatoryOptionalFieldValidation(false);
        }

        [Test, Description("")]
        public void _017_UpdateCompanyMandatoryOptionalFieldValidation()
        {
            base.View.SelectTier("CompanyToAddTo");
            base.View.ClickUpdate();

            CompanyMandatoryOptionalFieldValidation(true);
        }

        [Test, Description("")]
        public void _018_UpdateBranchMandatoryOptionalFieldValidation()
        {
            base.View.SelectTier("CompanyUpdateValidation");
            base.View.ClickUpdate();

            BranchMandatoryOptionalFieldValidation(true);
        }

        [Test, Description("")]
        public void _019_UpdateDepartmentMandatoryOptionalFieldValidation()
        {
            base.View.SelectTier("CompanyUpdateValidation", "BranchUpdateValidation");
            base.View.ClickUpdate();

            DepartmentMandatoryOptionalFieldValidation(true);
        }

        [Test, Description("")]
        public void _020_UpdateContactMandatoryOptionalFieldValidation()
        {
            base.View.SelectTier("CompanyUpdateValidation", "BranchUpdateValidation", "DepartmentUpdateValidation", "Mr Contact UpdateValidation");
            base.View.ClickUpdate();

            ContactMandatoryOptionalFieldValidation(true);
        }

        [Test, Description("")]
        public void _021_RemoveParentNodevblidation()
        {
            base.View.SelectTier("CompanyRemoveParentNodevblidation");
            base.View.ClickUpdate();
            base.Browser.Page<MaintenanceLegalEntity>().ClickRemove();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Cannot remove this node as it is linked to active child nodes.");
        }

        [Test, Description("")]
        public void _022_AddPDABankDetails()
        {
            string leName = "AddPDABankDetails";
            base.View.SelectTier(leName);
            base.View.AddToMenu();

            base.Browser.Navigate<LoanServicingCBO>().LegalEntityParentNode(leName);
            base.Browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Add);
            var bankAccount = Service<IBankingDetailsService>().GetNextUnusedBankAccountDetails();
            bankAccount.AccountName = leName;
            base.Browser.Page<BankingDetails>().AddBankingDetails(bankAccount, ButtonTypeEnum.Add);
            base.Browser.Page<BankingDetailsDisplay>().ValidateBankingDetailsDisplayed(bankAccount.ACBBankDescription, bankAccount.ACBTypeDescription, bankAccount.AccountNumber, bankAccount.AccountName, "Active");
        }

        #region Helpers

        private void CompanyMandatoryOptionalFieldValidation(bool update)
        {
            var leos = base.Service<ILegalEntityService>().GetEmptyLegalEntityOrganisationStructure();

            leos.LegalEntity.LegalEntityTypeDescription = "Company";
            leos.OrganisationStructure.OrganisationTypeDescription = "- Please Select -";
            leos.LegalEntity.RegisteredName = "";
            leos.LegalEntity.FaxCode = "";
            leos.LegalEntity.FaxNumber = "";
            leos.LegalEntity.WorkPhoneCode = "";
            leos.LegalEntity.WorkPhoneNumber = "";
            leos.LegalEntity.EmailAddress = "";

            base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
            if (update)
                base.Browser.Page<MaintenanceLegalEntity>().Update();
            else
                base.Browser.Page<MaintenanceLegalEntity>().Add();

            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Organisation Type is required.");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Company Name is required.");

            leos.LegalEntity.RegisteredName = "CompanyMandatoryOptionalFieldValidation";
            leos.OrganisationStructure.OrganisationTypeDescription = "Company";
            base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
            if (update)
                base.Browser.Page<MaintenanceLegalEntity>().Update();
            else
                base.Browser.Page<MaintenanceLegalEntity>().Add();

            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Legal Entity Registration Number Required");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                "At least one contact number must be provided. Note that for non-cellphone numbers both a dialing code and number must be provided");

            if (!update)
            {
                leos.LegalEntity.RegistrationNumber = string.Format(@"ABC{0}/DEF{1}", random.Next(0, 99999), random.Next(0, 100));
                leos.LegalEntity.WorkPhoneCode = "012";
                leos.LegalEntity.WorkPhoneNumber = "1234567";
                leos.LegalEntity.EmailAddress = "test@test.com";

                base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
                if (update)
                    base.Browser.Page<MaintenanceLegalEntity>().Update();
                else
                    base.Browser.Page<MaintenanceLegalEntity>().Add();
                base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("When no unit number, building number or building name is supplied, Street Number is required.",
                    "A street number, building number or unit number is required.",
                    "Please enter a valid street name.",
                    "Street Name is a mandatory field",
                    "Suburb is a mandatory field");
            }
        }

        private void BranchMandatoryOptionalFieldValidation(bool update)
        {
            var leos = base.Service<ILegalEntityService>().GetEmptyLegalEntityOrganisationStructure();

            leos.LegalEntity.LegalEntityTypeDescription = "Company";
            leos.OrganisationStructure.OrganisationTypeDescription = "Branch";
            leos.LegalEntity.RegisteredName = "";
            leos.LegalEntity.FaxCode = "";
            leos.LegalEntity.FaxNumber = "";
            leos.LegalEntity.WorkPhoneCode = "";
            leos.LegalEntity.WorkPhoneNumber = "";
            leos.LegalEntity.EmailAddress = "";

            base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
            if (update)
                base.Browser.Page<MaintenanceLegalEntity>().Update();
            else
                base.Browser.Page<MaintenanceLegalEntity>().Add();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Branch Name is required.");

            leos.LegalEntity.RegisteredName = "BranchMandatoryOptionalFieldValidation";

            base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
            if (update)
                base.Browser.Page<MaintenanceLegalEntity>().Update();
            else
                base.Browser.Page<MaintenanceLegalEntity>().Add();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                 "At least one contact number must be provided. Note that for non-cellphone numbers both a dialing code and number must be provided");

            if (!update)
            {
                leos.LegalEntity.WorkPhoneCode = "012";
                leos.LegalEntity.WorkPhoneNumber = "1234567";
                leos.LegalEntity.EmailAddress = "test@test.com";

                base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
                if (update)
                    base.Browser.Page<MaintenanceLegalEntity>().Update();
                else
                    base.Browser.Page<MaintenanceLegalEntity>().Add();

                base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("When no unit number, building number or building name is supplied, Street Number is required.",
                     "A street number, building number or unit number is required.",
                     "Please enter a valid street name.",
                     "Street Name is a mandatory field",
                     "Suburb is a mandatory field");
            }
        }

        private void DepartmentMandatoryOptionalFieldValidation(bool update)
        {
            var leos = base.Service<ILegalEntityService>().GetEmptyLegalEntityOrganisationStructure();

            leos.LegalEntity.LegalEntityTypeDescription = "Company";
            leos.OrganisationStructure.OrganisationTypeDescription = "Department";
            leos.LegalEntity.RegisteredName = "";
            leos.LegalEntity.FaxCode = "";
            leos.LegalEntity.FaxNumber = "";
            leos.LegalEntity.WorkPhoneCode = "";
            leos.LegalEntity.WorkPhoneNumber = "";
            leos.LegalEntity.EmailAddress = "";

            base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
            if (update)
                base.Browser.Page<MaintenanceLegalEntity>().Update();
            else
                base.Browser.Page<MaintenanceLegalEntity>().Add();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Department Name is required.");

            leos.LegalEntity.RegisteredName = "DepartmentMandatoryOptionalFieldValidation";

            base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
            if (update)
                base.Browser.Page<MaintenanceLegalEntity>().Update();
            else
                base.Browser.Page<MaintenanceLegalEntity>().Add();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                "At least one contact number must be provided. Note that for non-cellphone numbers both a dialing code and number must be provided");
        }

        private void ContactMandatoryOptionalFieldValidation(bool update)
        {
            var leos = base.Service<ILegalEntityService>().GetEmptyLegalEntityOrganisationStructure();
            leos.LegalEntity.LegalEntityTypeDescription = "Natural Person";
            leos.OrganisationStructure.OrganisationTypeDescription = "- Please Select -";
            leos.LegalEntity.SalutationDescription = "Mr";
            leos.LegalEntity.FirstNames = "ContactMandatoryOptional";
            leos.LegalEntity.Surname = "FieldValidation";
            leos.LegalEntity.GenderDescription = "Male";

            base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
            if (update)
                base.Browser.Page<MaintenanceLegalEntity>().Update();
            else
                base.Browser.Page<MaintenanceLegalEntity>().Add();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Role is a required item");

            leos.OrganisationStructure.OrganisationTypeDescription = "Contact";
            leos.LegalEntity.SalutationDescription = "- Please Select -";

            base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
            if (update)
                base.Browser.Page<MaintenanceLegalEntity>().Update();
            else
                base.Browser.Page<MaintenanceLegalEntity>().Add();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("At least one contact detail (Email, Home, Work or Cell Number) is required");

            leos.LegalEntity.SalutationDescription = "Mr";
            leos.LegalEntity.FirstNames = "";
            leos.LegalEntity.Surname = "";
            base.Browser.Page<MaintenanceLegalEntity>().PopulateLegalEntity(leos);
            if (update)
                base.Browser.Page<MaintenanceLegalEntity>().Update();
            else
                base.Browser.Page<MaintenanceLegalEntity>().Add();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Legal Entity First Name Required");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Legal Entity Surname Required");
        }

        #endregion Helpers
    }
}