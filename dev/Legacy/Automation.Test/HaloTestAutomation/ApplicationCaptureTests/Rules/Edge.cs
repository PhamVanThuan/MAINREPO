using BuildingBlocks;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace ApplicationCaptureTests.Rules
{
    [TestFixture, RequiresSTA]
    public class Edge : ApplicationCaptureTests.TestBase<ApplicationLoanDetailsUpdate>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.BranchConsultant);
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            if (base.Browser != null)
            {
                base.Browser.Page<BasePage>().CheckForErrorMessages();
            }
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            Settings.WaitForCompleteTimeOut = 120;
            if (base.Browser != null)
            {
                base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
                base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
                base.Browser.Navigate<BuildingBlocks.Navigation.CalculatorsNode>().Calculators(base.Browser, CalculatorNodeTypeEnum.ApplicationCalculator);
            }
        }

        /// <summary>
        /// This tests the rule that ensures an Edge application with a loan amount greater than R2.5 mil is prevented from continuing through
        /// the origination process without being reworked to an amount less than or equal to R2.5 mil. This test ensures that the rule only
        /// executes for those cases where the loan amount exceeds R2.5 mil.
        /// </summary>
        [Test, Description("Ensures an Edge application does not exceed the maximum loan amount")]
        public void when_the_edge_application_LTV_is_below_80_percent_the_LAA_should_be_limited_to_2500000()
        {
            //complete the calculator
            var offerKey = CreateEdgeApplication(3500000, 2500000, 105000);
            //submit application
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.SubmitApplication);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //assert error message not displayed
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageDoesNotExist("The maximum Loan Agreement amount for Edge is R2500000.");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageDoesNotExist(@"A maximum loan amount of R 1 500 000.00 is allowed on Edge products where LTV is greater than or equal to 80.00%. ");
            //rework the cash out value
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
            base.Browser.Page<ApplicationLoanDetailsUpdate>().ChangeRefinanceCashOutValue("2499999.99");
            base.Browser.Page<ApplicationLoanDetailsUpdate>().RecalcAndSave(false);
            //submit application
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.SubmitApplication);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //assert error message not displayed
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageDoesNotExist("The maximum Loan Agreement amount for Edge is R2500000.");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageDoesNotExist(@"A maximum loan amount of R 1 500 000.00 is allowed on Edge products where LTV is greater than or equal to 80.00%. ");
            //rework the cash out value
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
            base.Browser.Page<ApplicationLoanDetailsUpdate>().ChangeRefinanceCashOutValue("2500000.01");
            base.Browser.Page<ApplicationLoanDetailsUpdate>().RecalcAndSave(false);
            //submit application
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.SubmitApplication);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //assert error message displayed
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The maximum Loan Agreement amount for Edge is R2500000.");
        }

        /// <summary>
        /// This test will ensure that a warning is displayed if the LTV on an edge application is < 80%
        /// </summary>
        [Test, Description("This test will ensure that a warning is displayed if the LTV on an edge application is < 80%")]
        public void when_the_edge_application_LTV_exceeds_80_percent_the_expected_rule_should_run()
        {
            string expectedMessage = @"Edge applications require an LTV of < 80%";
            base.Browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_Refinance(Products.Edge, "1829500", "1500000", EmploymentType.Salaried,
                null, false, "105000", ButtonTypeEnum.Calculate);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(expectedMessage);
        }

        private int CreateEdgeApplication(int marketValue, int cashRequired, int householdIncome)
        {
            //complete the calculator
            string idNumber = IDNumbers.GetNextIDNumber();
            base.Browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_Refinance(Products.Edge, marketValue.ToString(), cashRequired.ToString(), EmploymentType.Salaried, 
                null, false, householdIncome.ToString(), ButtonTypeEnum.CreateApplication);
            //add the LE
            base.Browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(OfferRoleTypes.LeadMainApplicant, true, idNumber, "Mr", "E", "Edge", "BlueBanner",
                null, Gender.Male, MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, null, null, null, "Unknown", Language.English,
                null, null, null, null, null, null, null, "0824968726", null, false, false, false, true, false,
                ButtonTypeEnum.Next);
            var offerKey = base.Browser.Page<ApplicationSummaryBase>().GetOfferKey();
            Assert.That(offerKey > 0, "Offer not created");
            return offerKey;
        }
    }
}