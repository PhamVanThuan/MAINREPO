using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace ApplicationCaptureTests.Views
{
    [TestFixture, RequiresSTA]
    public class ApplicationLoanCalculator : TestBase<BuildingBlocks.Views.LoanCalculator>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.BranchConsultant);
        }

        protected override void OnTestStart()
        {
            //application calculator
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.CalculatorsNode>().Calculators(base.Browser, CalculatorNodeTypeEnum.ApplicationCalculator);
        }

        /// <summary>
        /// This tests that the Term text field on the Application Loan Calculator screen defaults to the correct value depending on which
        /// Product is selected in the product dropdown.
        /// </summary>
        [Test, Description("Ensures the correct default term value is inserted per product")]
        public void _01_DefaultTerm()
        {
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
        /// This test creates an application and checks the Estate Agent checkbox on the Application Calculator. This requires an Estate Agent to be assigned to the
        /// application at the Estate Agent Application state in the workflow.
        /// </summary>
        [Test, Description("An Estate Agent Application must be routed differently through the Application Capture workflow in order to capture an Estate Agent role")]
        public void _02_CalculatorCreateEstateAgencyApplication()
        {
            base.Browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_Refinance(Products.NewVariableLoan, "850000", "250000", EmploymentType.Salaried,
                "240",  true, "40000", ButtonTypeEnum.CreateApplication, true);
            base.Browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(OfferRoleTypes.LeadMainApplicant, true, IDNumbers.GetNextIDNumber(), "Mr", "auto", "Estate",
                "Agency", "auto", Gender.Male, MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, "auto", null, null, "Unknown", Language.English, null,
                "011", "1234567", null, null, null, null, null, null, true, false, false, false, false, ButtonTypeEnum.Next);
            int offerKey = base.Browser.Page<ApplicationSummaryBase>().GetOfferKey();
            //this case should be at the correct state
            X2Assertions.AssertCurrentAppCapX2State(offerKey, WorkflowStates.ApplicationCaptureWF.EstateAgentApplication);
        }
    }
}