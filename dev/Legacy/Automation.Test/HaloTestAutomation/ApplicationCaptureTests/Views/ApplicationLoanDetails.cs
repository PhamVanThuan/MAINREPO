using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Threading;
using System.Linq;
using System;

namespace ApplicationCaptureTests.Views
{
    [TestFixture, RequiresSTA]
    public class ApplicationLoanDetails : TestBase<ApplicationLoanDetailsUpdate>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.BranchConsultant);
            base.Browser.Navigate<NavigationHelper>().CloseLoanNodesFLOBO(Browser);
        }

        protected override void OnTestStart()
        {
            //application calculator
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.CalculatorsNode>().Calculators(base.Browser, CalculatorNodeTypeEnum.ApplicationCalculator);
        }

        /// <summary>
        /// This tests that the validation error message executes ensuring that the Term field is mandatory. This test ensures that the Term for
        /// an application can not be reworked to a blank or zero value.
        /// </summary>
        [Test, Description("Ensures that an application can not have a blank or zero term")]
        public void _001_MinTermRequired()
        {
            //complete the calculator
            base.Browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_Refinance("New Variable Loan", "978500", "450000", "Salaried", null, false, "28500",
                ButtonTypeEnum.CreateApplication);
            //add the LE
            base.Browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(OfferRoleTypes.LeadMainApplicant, true, null, "Miss", "auto", "TestName", "TestSurname", null, Gender.Female,
                MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, null, null, null, "Unknown", Language.English, null, null, null, null, null, null,
                null, "0761948851", null, false, false, false, true, false, ButtonTypeEnum.Next);
            int offerKey = 0;
            offerKey = base.Browser.Page<ApplicationSummaryBase>().GetOfferKey();
            //assert offer created
            Assert.That(offerKey != 0, "Offer not created");
            //rework the term value to zero
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
            base.View.ChangeTerm("0");
            base.View.RecalculateApplication();
            //assert that the error message is displayed
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Term is a required field and must be greater than 0.");
            //rework the term value to blank
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
            base.View.ChangeTerm(string.Empty);
            base.View.RecalculateApplication();
            //assert that the error message is displayed
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Term is a required field and must be greater than 0.");
        }

        /// <summary>
        /// This tests that the Term text field on the Application Loan Details Update screen defaults to the correct value depending on which
        /// Product is selected in the product dropdown.
        /// </summary>
        [Test, Description("Ensures the correct default term value is inserted per product")]
        public void _002_DefaultTerm()
        {
            //complete the calculator
            base.Browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_Refinance("New Variable Loan", "768500", "355000", "Salaried", null, false, "26500",
                ButtonTypeEnum.CreateApplication);
            //add the LE
            base.Browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(OfferRoleTypes.LeadMainApplicant, true, null, "Mr", "auto", "Jake", "King", null,
                Gender.Male, MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, null, null, null, "Unknown", Language.English, null, null, null,
                null, null, null, null, "0847541479", null, false, false, false, true, false, ButtonTypeEnum.Next);
            int offerKey = 0;
            offerKey = base.Browser.Page<ApplicationSummaryBase>().GetOfferKey();
            //assert offer created
            Assert.That(offerKey != 0, "Offer not created");
            //rework the product to Edge
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
            base.View.ReworkProduct(Products.Edge);
            Thread.Sleep(2000);
            //assert on screen term value
            base.View.AssertTermValue(276);
            //rework the product to New Variable Loan
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
            base.View.ReworkProduct(Products.NewVariableLoan);
            Thread.Sleep(2000);
            //assert on screen term value
            base.View.AssertTermValue(240);
            //rework the product to Edge
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
            base.View.ReworkProduct(Products.Edge);
            Thread.Sleep(2000);
            //assert on screen term value
            base.View.AssertTermValue(276);
            //rework the product to New Variable Loan
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
            base.View.ReworkProduct(Products.NewVariableLoan);
            Thread.Sleep(2000);
            //assert on screen term value
            base.View.AssertTermValue(240);
        }

        [Test, Description("Asserts that the initiation fees are capitalised when the checkbox is ticked.")]
        public void _003_WhenCheckingCapitaliseInitiationFeesCheckboxShouldCapitaliseInitationFees()
        {
            int offerKey = LoadAlphaHousingOfferWithoutCapitalisedInitiationFee();

            int expectedTotalLoanRequired = base.View.GetTotalLoanRequired() + base.View.GetInitiationFee();
            base.View.CapitaliseInitiationFees(true);
            int actualTotalLoanRequired = base.View.GetTotalLoanRequired();

            Assert.True(expectedTotalLoanRequired.Equals(actualTotalLoanRequired));
            OfferAssertions.AssertOfferAttributeExists(offerKey, OfferAttributeTypeEnum.CapitaliseInitiationFee, false);
        }

        [Test, Description("Asserts that the initiation fees are capitalised when the checkbox is ticked and the recalculate button is clicked.")]
        public void _004_WhenCheckingCapitaliseInitiationFeesCheckboxAndPressingTheRecalculateButtonShouldCapitaliseInitationFees()
        {
            int offerKey = LoadAlphaHousingOfferWithoutCapitalisedInitiationFee();

            int expectedTotalLoanRequired = base.View.GetTotalLoanRequired() + base.View.GetInitiationFee();
            base.View.CapitaliseInitiationFees(true);
            base.View.RecalculateApplication();
            int actualTotalLoanRequired = base.View.GetTotalLoanRequired();

            Assert.True(expectedTotalLoanRequired.Equals(actualTotalLoanRequired));
            OfferAssertions.AssertOfferAttributeExists(offerKey, OfferAttributeTypeEnum.CapitaliseInitiationFee, false);
        }

        [Test, Description("Asserts that the initiation fees are capitalised and the expected offer attribute added when the checkbox is ticked and the save button is clicked.")]
        public void _005_WhenCheckingCapitaliseInitiationFeesCheckboxAndPressingTheSaveButtonShouldCapitaliseInitationFees()
        {
            int offerKey = LoadAlphaHousingOfferWithoutCapitalisedInitiationFee();
            int expectedTotalLoanRequired = base.View.GetTotalLoanRequired() + base.View.GetInitiationFee();
            base.View.CapitaliseInitiationFees(true);
            base.View.SaveApplication();
            double loanAgreementAmount = Service<IApplicationService>().GetLoanAgreementAmount(offerKey);

            OfferAssertions.AssertOfferAttributeExists(offerKey, OfferAttributeTypeEnum.CapitaliseInitiationFee, true);
            Assert.True(loanAgreementAmount.Equals(expectedTotalLoanRequired));
        }

        private int LoadAlphaHousingOfferWithoutCapitalisedInitiationFee()
        {
            int offerKey = base.Service<IApplicationService>().GetAlphaOffersAtAppCapWithoutCapitalisedInitiationFeeOfferAttribute().FirstOrDefault().OfferKey;
            
            base.Browser.Navigate<NavigationHelper>().Task();
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);

            return offerKey;
        }
       
    }
}