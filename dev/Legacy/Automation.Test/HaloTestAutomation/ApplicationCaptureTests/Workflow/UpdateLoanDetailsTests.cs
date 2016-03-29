using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace ApplicationCaptureTests.Workflow
{
    [RequiresSTA]
    public class UpdateLoanDetailsTests : ApplicationCaptureTests.TestBase<ApplicationLoanDetailsUpdate>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.BranchConsultant);
        }

        /// <summary>
        /// Verify that a Branch Consultant User can succesfully perform the Update Loan Details action
        /// </summary>
        [Test, Description("Verify that a Branch Consultant User can succesfully perform the Update Loan Details action")]
        public void _01_UpdateLoanDetailsBranchConsultant()
        {
            base.GetTestCase("UpdateLoanDetailsBranchConsultant");
            base.SearchForCase();
            //select the action
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
            //update the details
            base.TestCase.CashOut = (Convert.ToInt32(base.TestCase.CashOut) + 10000).ToString();
            base.TestCase.ExistingLoan = (Convert.ToInt32(base.TestCase.ExistingLoan) + 10000).ToString();
            base.View.UpdateSwitchLoanDetails(base.TestCase.CashOut, base.TestCase.ExistingLoan);
            //navigate the loan details screen
            Browser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            Browser.Navigate<LoanDetailsNode>().ClickLoanDetailsSummaryNode();
            //assert the details have changed
            Browser.Page<ApplicationLoanDetailsDisplay>().AssertSwitchLoanDetailsUpdate(base.TestCase.ExistingLoan, base.TestCase.CashOut);
        }

        /// <summary>
        /// Update the Application Details to apply a discounted link rate to the application
        /// </summary>
        /// <param name="value">Discount to be applied</param>
        [Test, Sequential, Description("Update the Application Details to apply a discounted link rate to the application")]
        public void _02_UpdateLoanDetailsApplyDiscountedLinkRate([Values(-0.5, 0.4)] double value)
        {
            base.GetTestCase("UpdateLoanDetails");
            base.SearchForCase();
            //select the action
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
            //apply the discounted link rate
            base.View.ApplyDiscountedLinkRate(value);
            //Assert that the OfferInformationRateOverride record exists for our discounted link rate
            ApplicationLoanDetailsAssertions.AssertOfferInformationFinancialAdjustmentExists(base.TestCase.OfferKey, FinancialAdjustmentTypeSourceEnum.Origination_InterestRateAdjustment, true, value);
        }

        /// <summary>
        /// Update the Application Details to apply Interest Only to the application
        /// </summary>
        [Test, Description("Update the Application Details to apply Interest Only to the application")]
        public void _03_UpdateLoanDetailsApplyInterestOnly()
        {
            base.GetTestCase("UpdateLoanDetails");
            string date = Service<IControlService>().GetControlTextValue("Interest Only Switch Off Date");
            DateTime offerStartDate = Convert.ToDateTime(date).AddDays(-1);
            Service<IApplicationService>().UpdateOfferStartDate(offerStartDate, base.TestCase.OfferKey);
            base.SearchForCase();
            //select the action
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
            //select the option
            base.View.UpdateInterestOnlyAttribute(true);
            //assert that the Interest Only record has been created
            ApplicationLoanDetailsAssertions.AssertOfferInformationFinancialAdjustmentExists(base.TestCase.OfferKey, FinancialAdjustmentTypeSourceEnum.InterestOnly_InterestOnly,
                false, 0.0);
        }

        /// <summary>
        /// Test ensures that a user can override the cancellation fee in the loan details screen
        /// </summary>
        [Test, Description("Test ensures that a user can override the cancellation fee in the loan details screen")]
        public void _04_UpdateLoanDetailsOverrideCancellationFee()
        {
            base.GetTestCase("UpdateLoanDetailsBranchConsultant");
            base.SearchForCase();
            //select the action
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
            //update the cancellation fee
            base.View.OverrideCancellationFee("2099", true);
            //assert that the fee has been updated
            OfferAssertions.AssertOfferExpense(base.TestCase.OfferKey, 2099.00f, true, ExpenseTypeEnum.CancellationFee);
        }

        /// <summary>
        /// Test ensures that the product of an application can be changed and that a revision is created
        /// </summary>
        /// <param name="product">Product to be revised to</param>
        /// <param name="revision">Revision number that will be created</param>
        [Test, Sequential, Description("Test ensures that the product of an application can be changed and that a revision is created")]
        public void _05_UpdateLoanDetailsChangeApplicationProduct([Values(Products.Edge, Products.NewVariableLoan)] string product, [Values(2, 3)] int revision)
        {
            base.GetTestCase("UpdateLoanDetailsRevisions");
            base.SearchForCase();
            //select the action
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
            //change the product
            base.View.ChangeApplicationProduct(product);
            //assert that the latest OI has been updated
            OfferAssertions.AssertLatestOfferInformationProduct(base.TestCase.OfferKey, product);
            //assert the revision has been created
            OfferAssertions.AssertOfferInformationCount(base.TestCase.OfferKey, revision);
        }
    }
}