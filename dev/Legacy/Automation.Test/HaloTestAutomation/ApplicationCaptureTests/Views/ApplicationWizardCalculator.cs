using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace ApplicationCaptureTests.Views
{
    [TestFixture, RequiresSTA]
    public class ApplicationWizardCalculator : ApplicationCaptureTests.TestBase<BuildingBlocks.Views.LoanCalculator>
    {
        protected override void OnTestStart()
        {
            base.OnTestStart();
            Service<IWatiNService>().CloseAllOpenIEBrowsers();
        }

        /// <summary>
        /// This tests that the Term text field on the Application Wizard Calculator screen defaults to the correct value depending on which
        /// Product is selected in the product dropdown.
        /// </summary>
        [Test, Description("Ensures the correct default term value is inserted per product")]
        public void _01_DefaultTerm()
        {
            base.Browser = new TestBrowser(TestUsers.BranchConsultant10);
            //application wizard calculator
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.CalculatorsNode>().Calculators(base.Browser, CalculatorNodeTypeEnum.ApplicationWizard);
            //add the LE
            base.Browser.Page<NaturalPersonWizardCreate>().ApplicationWizardApplicant("Billboards", "", "John", "Henry", "031", "5607841");
            //select the needs indentification
            base.View.SelectNeedsIndentification("New Home Purchase");
            //select a product
            base.View.SelectProduct(Products.NewVariableLoan);
            //assert on screen term value
            base.View.AssertTermValue(240);
            //change the product to Edge
            base.View.SelectProduct(Products.Edge);
            //assert on screen term value
            base.View.AssertTermValue(276);
            //change the product to New Variable Loan
            base.View.SelectProduct(Products.NewVariableLoan);
            //assert on screen term value
            base.View.AssertTermValue(240);
            //change the product to Edge
            base.View.SelectProduct(Products.Edge);
            //assert on screen term value
            base.View.AssertTermValue(276);
            //change the product to New Variable Loan
            base.View.SelectProduct(Products.NewVariableLoan);
            //assert on screen term value
            base.View.AssertTermValue(240);
        }

        /// <summary>
        /// This test ensures that a branch consultant user can crate an application using the wizard.
        /// </summary>
        [Test, Description("Verify that a BCUser can create an application using the Wizard")]
        public void _02_CreateBranchConsultantWizardApplication()
        {
            string user = TestUsers.BranchConsultant;
            base.Browser = new TestBrowser(user);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);

            const string marketingSource = "Facebook";
            const string idNumber = "";
            const string firstname = "BranchConsultant";
            const string surname = "WizardApplication";
            const string phoneCode = "031";
            const string phoneNumber = "123456789";
            const string loanPurpose = "New purchase";
            const string product = "New Variable Loan";
            const string marketValue = "1000000";
            const string cashDeposit = "500000";
            const string employmentType = "Salaried";
            const string term = "";
            const string percentageToFix = "";
            const string housholdIncome = "100000";
            const string needsID = "New Home Purchase";

            int offerKey = CreateApplicationUsingWizard(marketingSource, idNumber, firstname, surname, phoneCode, phoneNumber, loanPurpose, product, marketValue, cashDeposit, employmentType,
                term, housholdIncome, needsID);

            //Assertions
            //commisionable consultant
            AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.CommissionableConsultant);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.CommissionableConsultant);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(offerKey, OfferRoleTypeEnum.CommissionableConsultant, user);
            //branch consultant
            string workflowState = WorkflowStates.ApplicationCaptureWF.ApplicationCapture;
            CheckCaseCreation(user, offerKey, workflowState, OfferRoleTypeEnum.BranchConsultantD);
        }

        /// <summary>
        /// This test ensures that a branch admin user can crate an application using the wizard.
        /// </summary>
        [Test, Description("Verify that a BAUser can create an application using the Wizard")]
        public void _03_CreateBranchAdminWizardApplication()
        {
            string testUser = TestUsers.BranchAdmin;
            base.Browser = new TestBrowser(testUser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);

            const string marketingSource = "Facebook";
            const string idNumber = "";
            const string firstname = "BranchAdmin";
            const string surname = "WizardApplication";
            const string phoneCode = "031";
            const string phoneNumber = "123456789";
            const string loanPurpose = "New purchase";
            const string product = "New Variable Loan";
            const string marketValue = "1000000";
            const string cashDeposit = "500000";
            const string employmentType = "Salaried";
            const string term = "";
            const string percentageToFix = "";
            const string housholdIncome = "100000";
            const string needsID = "New Home Purchase";

            int offerKey = CreateApplicationUsingWizard(marketingSource, idNumber, firstname, surname, phoneCode, phoneNumber, loanPurpose, product, marketValue, cashDeposit, employmentType,
                term, housholdIncome, needsID);

            //Assertions
            string workflowState = WorkflowStates.ApplicationCaptureWF.ConsultantAssignment;
            CheckCaseCreation(testUser, offerKey, workflowState, OfferRoleTypeEnum.BranchAdminD);
        }

        private int CreateApplicationUsingWizard(string marketingSource, string idNumber, string firstname, string surname, string phoneCode, string phoneNumber, string loanPurpose, string product, string marketValue,
            string cashDeposit, string employmentType, string term, string housholdIncome, string needsID)
        {
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.CalculatorsNode>().Calculators(base.Browser, CalculatorNodeTypeEnum.ApplicationWizard);

            base.Browser.Page<NaturalPersonWizardCreate>().ApplicationWizardApplicant(marketingSource, idNumber, firstname, surname, phoneCode, phoneNumber);

            switch (loanPurpose)
            {
                case "New purchase":
                    base.Browser.Page<BuildingBlocks.Views.LoanCalculator>().ApplicationWizardCalculator_NewPurchase(product, marketValue, cashDeposit, employmentType,
                        term, housholdIncome, needsID, ButtonTypeEnum.CreateApplication);
                    break;
            }

            const string streetNumber = "33";
            const string streetName = "WizardTest";
            const string province = "Kwazulu-natal";
            const string suburb = "Umhlanga";

            base.Browser.Page<ApplicationDeclarations>().ApplicationDeclarationsUpdate();
            base.Browser.Page<LegalEntityAddressDetails>().AddResidentialAddress(streetNumber, streetName, province, suburb, ButtonTypeEnum.Update);
            base.Browser.Page<LegalEntityAddressDetails>().AddResidentialAddress(ButtonTypeEnum.Update);
            base.Browser.Page<ApplicationWizardSummary>().ApplicationDetails(ButtonTypeEnum.Finish);
            int offerKey = base.Browser.Page<ApplicationSummaryBase>().GetOfferKey();
            return offerKey;
        }

        private static void CheckCaseCreation(string user, int offerKey, string workflowState, OfferRoleTypeEnum roleType)
        {
            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, roleType);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, roleType);
            AssignmentAssertions.AssertWhoTheWorkFlowAssignmentRecordIsAssignedTo(offerKey, roleType, user);
            AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, roleType);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, roleType);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(offerKey, roleType, user);
            AssignmentAssertions.AssertOfferExistsOnWorkList(offerKey, workflowState, user, Workflows.ApplicationCapture);
        }
    }
}