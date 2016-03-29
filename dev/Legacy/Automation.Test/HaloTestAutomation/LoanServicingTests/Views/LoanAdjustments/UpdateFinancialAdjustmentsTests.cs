using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace LoanServicingTests.Views.LoanAdjustments
{
    [RequiresSTA]
    public class UpdateFinancialAdjustments : TestBase<UpdateFinancialAdjustment>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.FLManager, TestUsers.Password);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
        }

        /// <summary>
        /// Update an active Defending Cancellation financial adjustment to inactive, assert that
        /// RateOverride is Inactive.
        /// Product remains unchanged.
        /// Write stagetransition (StageDefinitionKey=3196)
        /// Post loan transaction 920 (Rate Change) and 940 (Installment Change)
        /// Installment, Rate and Discount recalc.
        /// </summary>
        [Test, Description(@"Update an active Defending Cancellation financial adjustment to inactive, assert that
		RateOverride is Inactive.
		Product remains unchanged.
		Write stagetransition (StageDefinitionStageDefinitionGroupKey?=4191)
		Post loan transaction 920 (Rate Change) and 940 (Installment Change)
		Installment, Rate and Discount recalc.")]
        public void SetDefendingCancellationToCancelledFromActive()
        {
            var results = Service<IFinancialAdjustmentService>().GetAccountWithFinancialAdjustmentNotUnderDebtCounselling(
                FinancialAdjustmentTypeSourceEnum.DefendingCancellation_InterestRateAdjustment, FinancialAdjustmentStatusEnum.Active);
            int accountKey = results.Column("accountKey").GetValueAs<int>();
            string roTypeDescription = results.Column("TypeDescription").Value;
            string fromDate = results.Column("FromDate").Value;
            // Get Previous Values
            string mlDiscount = Service<IAccountService>().GetMortgageLoanColumn(accountKey, "rateAdjustment");
            string mlInteresRate = Service<IAccountService>().GetMortgageLoanColumn(accountKey, "InterestRate");
            string mlPayment = Service<IAccountService>().GetMortgageLoanColumn(accountKey, "Payment");
            //Start Testing
            NavigateToUpdateFinancialAdjustmentsScreen(accountKey);
            base.View.SetFinancialAdjustmentStatus(roTypeDescription, FinancialAdjustmentStatusEnum.Active, fromDate, FinancialAdjustmentStatusEnum.Canceled);
            base.View.Submit();
            // RateOverride is Inactive.
            int financialAdjustmentKey = FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DefendingCancellation_InterestRateAdjustment,
                FinancialAdjustmentStatusEnum.Canceled, true);
            // Product changes to new variable loan
            AccountAssertions.AssertAccountInformation(accountKey, "rrr_productKey", ((int)ProductEnum.NewVariableLoan).ToString());
            // check if stagetransition written - 4191
            StageTransitionAssertions.AssertStageTransitionCreated(financialAdjustmentKey, StageDefinitionStageDefinitionGroupEnum.LoanServicing_InactivatedRateOverride);
            FinancialAdjustmentAssertions.AssertFinancialAdjustmentUpdate(accountKey, mlDiscount, mlInteresRate, mlPayment, financialAdjustmentKey);
        }

        /// <summary>
        /// Update an Inactive Defending Cancellation financial adjustment to Active, assert that
        /// RateOverride is Active.
        /// Product remains unchanged.
        /// Write stagetransition (StageDefinitionKey=3196)
        /// Post loan transaction 920 (Rate Change) and 940 (Installment Change)
        /// Installment, Rate and Discount recalc.
        /// </summary>
        [Test, Description(@"Update an Cancelled Defending Cancellation financial adjustment to Active, assert that
		RateOverride is Active.
		Product remains unchanged.
		Write stagetransition (StageDefinitionStageDefinitionGroupKey?=4191)
		Post loan transaction 920 (Rate Change) and 940 (Installment Change)
		Installment, Rate and Discount recalc.")]
        public void SetDefendingCancellationToActiveFromCancelled()
        {
            var results = Service<IFinancialAdjustmentService>().GetAccountWithFinancialAdjustmentNotUnderDebtCounselling(FinancialAdjustmentTypeSourceEnum.DefendingCancellation_InterestRateAdjustment,
                FinancialAdjustmentStatusEnum.Canceled);
            int accountKey = results.Column("accountKey").GetValueAs<int>();
            string roTypeDescription = results.Column("TypeDescription").Value;
            string fromDate = results.Column("FromDate").Value;
            // Get Previous Values
            NavigateToUpdateFinancialAdjustmentsScreen(accountKey);
            base.View.SetFinancialAdjustmentStatus(roTypeDescription, FinancialAdjustmentStatusEnum.Canceled, fromDate, FinancialAdjustmentStatusEnum.Active);
            base.View.Submit();
            // Assert the warning message does appear
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Cannot opt into Defending Cancellations.");
        }

        /// <summary>
        /// RateOverride? is Inactive.
        /// Product remains unchanged.
        /// Write stagetransition (StageDefinitionKey?=3196)
        /// Post loan transaction 920 (Rate Change) and 940 (Installment Change)
        /// Installment, Rate and Discount recalc.
        /// </summary>
        [Test, Description(@"RateOverride? is Inactive.
			Product remains unchanged.
			Write stagetransition (StageDefinitionStageDefinitionGroupKey?=4191)
			Post loan transaction 920 (Rate Change) and 940 (Installment Change)
			Installment, Rate and Discount recalc.")]
        public void SetDiscountedLinkRateToCancelledFromActive()
        {
            var results = Service<IFinancialAdjustmentService>().GetAccountWithFinancialAdjustmentNotUnderDebtCounselling(
                    FinancialAdjustmentTypeSourceEnum.Origination_InterestRateAdjustment, FinancialAdjustmentStatusEnum.Active);
            int accountKey = results.Column("accountKey").GetValueAs<int>();
            string roTypeDescription = results.Column("TypeDescription").Value;
            string origProductKey = results.Column("productKey").Value;
            string fromDate = results.Column("FromDate").Value;
            // Get Previous Values
            string mlDiscount = Service<IAccountService>().GetMortgageLoanColumn(accountKey, "rateAdjustment");
            string mlInteresRate = Service<IAccountService>().GetMortgageLoanColumn(accountKey, "InterestRate");
            string mlPayment = Service<IAccountService>().GetMortgageLoanColumn(accountKey, "Payment");
            //Start Testing
            NavigateToUpdateFinancialAdjustmentsScreen(accountKey);
            base.View.SetFinancialAdjustmentStatus(roTypeDescription, FinancialAdjustmentStatusEnum.Active, fromDate, FinancialAdjustmentStatusEnum.Canceled);
            base.View.Submit();
            // RateOverride is Inactive.
            int financialAdjustmentKey = FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.Origination_InterestRateAdjustment,
                FinancialAdjustmentStatusEnum.Canceled, true);
            // Product remains unchanged
            AccountAssertions.AssertAccountInformation(accountKey, "rrr_productKey", origProductKey);
            // check if stagetransition written - 4191
            StageTransitionAssertions.AssertStageTransitionCreated(financialAdjustmentKey, StageDefinitionStageDefinitionGroupEnum.LoanServicing_InactivatedRateOverride);
            FinancialAdjustmentAssertions.AssertFinancialAdjustmentUpdate(accountKey, mlDiscount, mlInteresRate, mlPayment, financialAdjustmentKey);
        }

        /// <summary>
        /// RateOverride? is Inactive.
        /// Product remains unchanged.
        /// Write stagetransition (StageDefinitionKey?=3196)
        /// Post loan transaction 920 (Rate Change) and 940 (Installment Change)
        /// Installment, Rate and Discount recalc.
        /// </summary>
        [Test, Description(@"RateOverride? is Inactive.
			Product remains unchanged.
			Write stagetransition (StageDefinitionStageDefinitionGroupKey?=4191)
			Post loan transaction 920 (Rate Change) and 940 (Installment Change)
			Installment, Rate and Discount recalc.")]
        public void SetDiscountedLinkRateToActiveFromCancelled()
        {
            var results = Service<IFinancialAdjustmentService>().GetAccountWithFinancialAdjustmentNotUnderDebtCounselling(FinancialAdjustmentTypeSourceEnum.Origination_InterestRateAdjustment,
                FinancialAdjustmentStatusEnum.Canceled);
            int accountKey = results.Column("accountKey").GetValueAs<int>();
            string roTypeDescription = results.Column("TypeDescription").Value;
            string fromDate = results.Column("FromDate").Value;
            //Start Testing
            NavigateToUpdateFinancialAdjustmentsScreen(accountKey);
            base.View.SetFinancialAdjustmentStatus(roTypeDescription, FinancialAdjustmentStatusEnum.Canceled, fromDate, FinancialAdjustmentStatusEnum.Active);
            base.View.Submit();
            // We expect an inactive financial adjustment to exist
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.Origination_InterestRateAdjustment, FinancialAdjustmentStatusEnum.Inactive, true);
        }

        [Test]
        public void UpdateFinancialAdjustment_WhenSettingInactiveDiscountedLinkRateToActive_ShouldNotDoAnything()
        {
            var results = Service<IFinancialAdjustmentService>().GetAccountWithFinancialAdjustmentNotUnderDebtCounselling(
                    FinancialAdjustmentTypeSourceEnum.Origination_InterestRateAdjustment, FinancialAdjustmentStatusEnum.Inactive);
            int accountKey = results.Column("accountKey").GetValueAs<int>();
            string roTypeDescription = results.Column("TypeDescription").Value;
            string fromDate = results.Column("FromDate").Value;
            //Start Testing
            NavigateToUpdateFinancialAdjustmentsScreen(accountKey);
            base.View.SetFinancialAdjustmentStatus(roTypeDescription, FinancialAdjustmentStatusEnum.Inactive, fromDate, FinancialAdjustmentStatusEnum.Active);
            base.View.Submit();
            // We expect an inactive financial adjustment to exist
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.Origination_InterestRateAdjustment, FinancialAdjustmentStatusEnum.Inactive, true);
        }

        /// <summary>
        /// Navigates to the update financial adjustments screen
        /// </summary>
        /// <param name="accountKey"></param>
        private void NavigateToUpdateFinancialAdjustmentsScreen(int accountKey)
        {
            //load the client into the CBO
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().UpdateFinancialAdjustments();
        }
    }
}