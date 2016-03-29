using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DebtCounsellingTests.Workflow
{
    /// <summary>
    ///
    /// </summary>
    [TestFixture, RequiresSTA]
    public sealed class ProposalAcceptAndDecline : DebtCounsellingTests.TestBase<CommonReasonCommonDecline>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingSupervisor);
        }

        #region AcceptProposalTests

        /// <summary>
        /// Test field validation on Accept Proposal screen
        /// 1. Assert that a Reason is mandatory
        /// 2. Assert that only one reason is allowed to be captured on Submit
        /// 3. Assert that it is possible to submit an Accept Proposal Capture Reason without a Comment
        /// 3.1 Assert that the case is moved to the 'Accepted Proposal' X2 state
        /// </summary>
        [Test, Description("")]
        public void AcceptProposalFieldValidation()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.DecisiononProposal, TestUsers.DebtCounsellingSupervisor);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.Accept);
            //Select 2 Reasons and Submit
            base.Browser.Page<CommonReasonCommonDecline>().SelectMultipleReasons(ReasonType.ProposalAccepted, 2);
            //Assert rule is hit
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Only one reason can be selected");
        }

        /// <summary>
        /// Test that it is possible to Accept a Proposal with a maximum total term of 360 months
        /// 1 Assert that the case is moved to the 'Accepted Proposal' X2 state
        /// 2 Assert the case is assigned to the last active Debt Counselling Consultant user associated with the case
        /// 2.1 Check the [2AM]..WorkflowRole records
        /// 2.2 Check the X2.x2.WorkflowRoleAssignment and X2.x2.Worklist records
        /// 3 Assert that a Reason record is created with a comment
        /// </summary>
        [Test, Description("")]
        public void AcceptProposalTermGreaterThan360UpperBound()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.DecisiononProposal, TestUsers.DebtCounsellingSupervisor);
            Service<IProposalService>().UpdateProposalEndDate(base.TestCase.AccountKey, 360);
            AcceptProposal(ReasonDescription.ProposalAcceptance);
        }

        /// <summary>
        /// Test that it is NOT possible to Accept a Proposal with a total term that exceeds 360 months and the selected reason is 'Proposal Accpetance'
        /// 1 Assert that the case remains at the 'Decision on Proposal' X2 state
        /// 2 Assert the case remains with the current user
        /// 2.1 Check the [2AM]..WorkflowRole records
        /// 2.2 Check the X2.x2.WorkflowRoleAssignment and X2.x2.Worklist records
        /// </summary>
        [Test, Description("Test that it is NOT possible to Accept a Proposal with a total term that exceeds 360 months and the selected reason is 'Proposal Acceptance'")]
        public void AcceptProposalTermGreaterThan360UpperBoundExceeded()
        {
            int accountKey = 0;
            try
            {
                base.StartTest(WorkflowStates.DebtCounsellingWF.DecisiononProposal, TestUsers.DebtCounsellingSupervisor, product: ProductEnum.NewVariableLoan, isInterestOnly: false);
                accountKey = base.TestCase.AccountKey;
                Service<IProposalService>().UpdateProposalEndDate(accountKey, 361);
                base.Browser.ClickAction(WorkflowActivities.DebtCounselling.Accept);
                base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonDescription.ProposalAcceptance, ButtonTypeEnum.Submit);
                //Assertions
                base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The total term of the loan cannot exceed 360 months from the Registration Date.");
                DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.DecisiononProposal);
            }
            finally
            {
                Service<IProposalService>().UpdateProposalEndDate(accountKey, 360);
            }
        }

        /// <summary>
        /// Test that the 'Remaining Term' field on the Debt Counselling Summary screen is highlighted red when the Remaining Term is greater than the existing term prior to acceptance
        /// </summary>
        [Test, Description(@"Test that the 'Remaining Term' field on the Debt Counselling Summary screen is highlighted red when the Remaining Term is greater than the existing term prior to acceptance")]
        public void RemainingTermHighlightedRedWhenGreaterThanCurrentRemainingTerm()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.AcceptedProposal, TestUsers.DebtCounsellingConsultant);
            //Set Remaining Term to existing term prior to acceptance
            int remainingTerm = Service<IDebtCounsellingService>().GetSnapShotAccountByDebtCounsellingKey(base.TestCase.DebtCounsellingKey)
                .RemainingInstallments;
            Service<IAccountService>().UpdateRemainingTerm(base.TestCase.AccountKey, remainingTerm);
            //Assert Remaining Term is not highlighted
            base.Browser.ClickWorkflowLoanNode(Common.Constants.Workflows.DebtCounselling, base.TestCase.AccountKey);
            base.Browser.Page<DebtCounsellingSummaryReview>().AssertDebtCounsellingSummaryRemainingTermFieldHighlighted(base.TestCase.AccountKey, false);
            //Set Remaining Term to exceed existing term prior to acceptance
            remainingTerm += 1;
            Service<IAccountService>().UpdateRemainingTerm(base.TestCase.AccountKey, remainingTerm);
            //Assert Remaining Term is highlighted
            base.Browser.ClickWorkflowLoanNode(Common.Constants.Workflows.DebtCounselling, base.TestCase.AccountKey);
            base.Browser.Page<DebtCounsellingSummaryReview>().AssertDebtCounsellingSummaryRemainingTermFieldHighlighted(base.TestCase.AccountKey, true);
        }

        /// <summary>
        ///
        /// </summary>
        [Test, Sequential, Description(@"Once a debt counselling case has been accepted where the account is not a Standard Variable or New Variable loan the case should be
			sent to Send Loan Agreements state and the case should be opted out of its current product to become a new variable loan.")]
        public void AcceptProposalOptOutRequired([Values(ProductEnum.Edge, ProductEnum.SuperLo, ProductEnum.VariFixLoan)] ProductEnum productType,
                                                 [Values(false, false, false)] bool isInterestOnly)
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.DecisiononProposal, TestUsers.DebtCounsellingSupervisor, product: productType, isInterestOnly: isInterestOnly);
            AcceptProposal(ReasonDescription.ProposalAcceptance);
        }

        /// <summary>
        ///
        /// </summary>
        [Test, Description(@"Once a debt counselling case has been accepted where the account is not a Standard Variable or New Variable loan the case should be
			sent to Send Loan Agreements state and the case should be opted out of its current product to become a new variable loan.")]
        public void AcceptProposalInterestOnly()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.DecisiononProposal, TestUsers.DebtCounsellingSupervisor, product: ProductEnum.NewVariableLoan, isInterestOnly: true);
            AcceptProposal(ReasonDescription.ProposalAcceptance);
        }

        /// <summary>
        /// A staff loan rate override needs to be removed should it exist on a debt counselling case's account. these cases should be sent to the
        /// Send Loan Agreements state.
        /// </summary>
        [Test, Description(@"A staff loan rate override needs to be removed should it exist on a debt counselling case's account. these cases should be sent to the	Send Loan Agreements state.")]
        public void AcceptProposalStaffLoan()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.DecisiononProposal, TestUsers.DebtCounsellingSupervisor, product: ProductEnum.NewVariableLoan, isInterestOnly: false);
            //we need to cancel existing adjustments
            base.Service<IFinancialAdjustmentService>().CancelFinancialAdjustments(base.TestCase.AccountKey);
            //start a staff one
            base.Service<IFinancialAdjustmentService>().StartFinancialAdjustment(base.TestCase.AccountKey, FinancialAdjustmentTypeSourceEnum.Staff_InterestRateAdjustment, 0.00);
            AcceptProposal(ReasonDescription.ProposalAcceptance);
        }

        /// <summary>
        /// A Defending Cancellation rate override needs to be removed should it exist on a debt counselling case's account. these cases should be sent to the
        /// Send Loan Agreements state.
        /// </summary>
        [Test, Description(@"A Defending Cancellation rate override needs to be removed should it exist on a debt counselling case's account. these cases should be sent to the	Send Loan Agreements state.")]
        public void AcceptProposalDefendingCancellation()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.DecisiononProposal, TestUsers.DebtCounsellingSupervisor, product: ProductEnum.NewVariableLoan, isInterestOnly: false);
            //we need to cancel existing adjustments
            base.Service<IFinancialAdjustmentService>().CancelFinancialAdjustments(base.TestCase.AccountKey);
            //start a defending cancellations adjustment
            base.Service<IFinancialAdjustmentService>().StartFinancialAdjustment(base.TestCase.AccountKey, FinancialAdjustmentTypeSourceEnum.DefendingCancellation_InterestRateAdjustment, 0.00);
            AcceptProposal(ReasonDescription.ProposalAcceptance);
        }

        /// <summary>
        /// </summary>
        [Test, Description(@"")]
        public void AcceptProposalNonPerformingLoan()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.DecisiononProposal, TestUsers.DebtCounsellingSupervisor, product: ProductEnum.NewVariableLoan, isInterestOnly: false);
            //we need to cancel existing adjustments
            base.Service<IFinancialAdjustmentService>().CancelFinancialAdjustments(base.TestCase.AccountKey);
            //start a non performing adjustment
            base.Service<IFinancialAdjustmentService>().StartFinancialAdjustment(base.TestCase.AccountKey, FinancialAdjustmentTypeSourceEnum.SuspendedInterest_ReversalProvision_NonPerformingLoans, 0.00);
            AcceptProposal(ReasonDescription.ProposalAcceptance);
        }

        /// <summary>
        /// Once a debt counselling case has been accepted where the account is not a Standard Variable or New Variable loan the case should be
        /// sent to Send Loan Agreements state and the case should be opted out of its current product to become a new variable loan.
        /// </summary>
        [Test, Description(@"Once a debt counselling case has been accepted where the account is not a Standard Variable or New Variable loan the case should be
			sent to Send Loan Agreements state and the case should be opted out of its current product to become a new variable loan.")]
        public void AcceptProposalNoOptOutRequired()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.DecisiononProposal, TestUsers.DebtCounsellingSupervisor, product: ProductEnum.NewVariableLoan, isInterestOnly: false);
            AcceptProposal(ReasonDescription.ProposalAcceptance);
        }

        #endregion AcceptProposalTests

        #region AcceptProposalRateOverrideTests

        /// <summary>
        /// This test will accept a proposal where both the HOC and Life have been set to exclusive. It ensures that the required work is done by the stored
        /// procedure and the expected rate overrides are created. It will also check that the proposal is accepted and the case is sent back to the debt consultant.
        /// </summary>
        [Test, Description(@"This test will accept a proposal where both the HOC and Life have been set to exclusive. It ensures that the required work is done by the stored
		procedure and the expected rate overrides are created. It will also check that the proposal is accepted and the case is sent back to the debt consultant.")]
        public void AcceptProposalFixedPaymentExclusive()
        {
            List<int> accounts = Service<IDebtCounsellingService>().GetAutomationDebtCounsellingTestCase(ProductEnum.NewVariableLoan, false);
            var accountKey = Service<IAccountService>().GetAccountWithOpenRelatedProducts(accounts, false, false);
            string id = Service<IAccountService>().GetIDNumbersForRoleOnAccount(accountKey, RoleTypeEnum.MainApplicant, GeneralStatusEnum.Active)[0];
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant, idNumber: id, searchForCase: false);
            //insert the proposal
            Service<IProposalService>().InsertActiveProposal(base.TestCase.DebtCounsellingKey, 1, TestUsers.DebtCounsellingConsultant, 0, 0, 0);
            //send for approval
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.SendProposalForApprovalWithExistingProposal, base.TestCase.DebtCounsellingKey);
            base.LoadCase(WorkflowStates.DebtCounsellingWF.DecisiononProposal);
            AcceptProposal(ReasonDescription.ProposalAcceptance);
            //DebtCounsellingFixedPaymentExclusive
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_18, FinancialAdjustmentStatusEnum.Inactive, true);
            //DebtCounsellingFixedRate
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_FixedRateAdjustment, FinancialAdjustmentStatusEnum.Inactive, true);
            //DebtCounsellingDiscountRate
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_InterestRateAdjustment, FinancialAdjustmentStatusEnum.Inactive, true);
        }

        /// <summary>
        /// This test will accept a proposal where both the HOC and Life have been set to inclusive. It ensures that the required work is done by the stored
        /// procedure and the expected rate overrides are created. It will also check that the proposal is accepted and the case is sent back to the debt consultant.
        /// </summary>
        [Test, Description(@"This test will accept a proposal where both the HOC and Life have been set to inclusive. It ensures that the required work is done by the stored
        procedure and the expected rate overrides are created. It will also check that the proposal is accepted and the case is sent back to the debt consultant.")]
        public void AcceptProposalFixedPaymentInclusive()
        {
            List<int> accounts = Service<IDebtCounsellingService>().GetAutomationDebtCounsellingTestCase(ProductEnum.NewVariableLoan, false);
            var accountKey = Service<IAccountService>().GetAccountWithOpenRelatedProducts(accounts, true, true);
            string id = Service<IAccountService>().GetIDNumbersForRoleOnAccount(accountKey, RoleTypeEnum.MainApplicant, GeneralStatusEnum.Active)[0];
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant, idNumber: id, searchForCase: false);
            //insert the proposal
            Service<IProposalService>().InsertActiveProposal(base.TestCase.DebtCounsellingKey, 1, TestUsers.DebtCounsellingConsultant, 1, 1, 1);
            //send for approval
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.SendProposalForApprovalWithExistingProposal, base.TestCase.DebtCounsellingKey);
            base.LoadCase(WorkflowStates.DebtCounsellingWF.DecisiononProposal);
            //get the consultant due to be assigned
            AcceptProposal(ReasonDescription.ProposalAcceptance);
            //DebtCounsellingFixedPaymentInclHOCLife
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_17, FinancialAdjustmentStatusEnum.Inactive, true);
            //DebtCounsellingFixedRate
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_FixedRateAdjustment, FinancialAdjustmentStatusEnum.Inactive, true);
            //DebtCounsellingDiscountRate
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_InterestRateAdjustment, FinancialAdjustmentStatusEnum.Inactive, true);
        }

        /// <summary>
        /// This test will accept a proposal where only HOC to inclusive. It ensures that the required work is done by the stored procedure and the expected
        /// rate overrides are created. It will also check that the proposal is accepted and the case is sent back to the debt consultant.
        /// </summary>
        [Test, Description(@"This test will accept a proposal where only HOC to inclusive. It ensures that the required work is done by the stored procedure and the expected
        rate overrides are created. It will also check that the proposal is accepted and the case is sent back to the debt consultant.")]
        public void AcceptProposalFixedPaymentInclusiveHOC()
        {
            List<int> accounts = Service<IDebtCounsellingService>().GetAutomationDebtCounsellingTestCase(ProductEnum.NewVariableLoan, false);
            var accountKey = Service<IAccountService>().GetAccountWithOpenRelatedProducts(accounts, true, false);
            string id = Service<IAccountService>().GetIDNumbersForRoleOnAccount(accountKey, RoleTypeEnum.MainApplicant, GeneralStatusEnum.Active)[0];
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant, idNumber: id, searchForCase: false);
            //insert the proposal
            Service<IProposalService>().InsertActiveProposal(base.TestCase.DebtCounsellingKey, 1, TestUsers.DebtCounsellingConsultant, 1, 0, 0);
            //send for approval
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.SendProposalForApprovalWithExistingProposal, base.TestCase.DebtCounsellingKey);
            base.LoadCase(WorkflowStates.DebtCounsellingWF.DecisiononProposal);
            AcceptProposal(ReasonDescription.ProposalAcceptance);
            //DebtCounsellingFixedPaymentInclusiveHOC
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_15, FinancialAdjustmentStatusEnum.Inactive, true);
            //DebtCounsellingFixedRate
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_FixedRateAdjustment, FinancialAdjustmentStatusEnum.Inactive, true);
            //DebtCounsellingDiscountRate
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_InterestRateAdjustment, FinancialAdjustmentStatusEnum.Inactive, true);
        }

        /// <summary>
        /// This test will accept a proposal where only Life to inclusive. It ensures that the required work is done by the stored procedure and the expected
        /// rate overrides are created. It will also check that the proposal is accepted and the case is sent back to the debt consultant.
        /// </summary>
        [Test, Description(@"This test will accept a proposal where only Life to inclusive. It ensures that the required work is done by the stored procedure and the expected
        rate overrides are created. It will also check that the proposal is accepted and the case is sent back to the debt consultant.")]
        public void AcceptProposalFixedPaymentInclusiveLife()
        {
            List<int> accounts = Service<IDebtCounsellingService>().GetAutomationDebtCounsellingTestCase(ProductEnum.NewVariableLoan, false);
            var accountKey = Service<IAccountService>().GetAccountWithOpenRelatedProducts(accounts, false, true);
            string id = Service<IAccountService>().GetIDNumbersForRoleOnAccount(accountKey, RoleTypeEnum.MainApplicant, GeneralStatusEnum.Active)[0];
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant, idNumber: id, searchForCase: false);
            //insert the proposal
            Service<IProposalService>().InsertActiveProposal(base.TestCase.DebtCounsellingKey, 1, TestUsers.DebtCounsellingConsultant, 0, 1, 0);
            //send for approval
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.SendProposalForApprovalWithExistingProposal, base.TestCase.DebtCounsellingKey);
            base.LoadCase(WorkflowStates.DebtCounsellingWF.DecisiononProposal);
            AcceptProposal(ReasonDescription.ProposalAcceptance);
            //DebtCounsellingFixedPaymentInclusiveLife
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_16, FinancialAdjustmentStatusEnum.Inactive, true);
            //DebtCounsellingFixedRate
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_FixedRateAdjustment, FinancialAdjustmentStatusEnum.Inactive, true);
            //DebtCounsellingDiscountRate
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_InterestRateAdjustment, FinancialAdjustmentStatusEnum.Inactive, true);
        }

        /// <summary>
        /// This test will accept a proposal where only MonthlyServiceFee to inclusive. It ensures that the required work is done by the stored procedure and the expected
        /// rate overrides are created. It will also check that the proposal is accepted and the case is sent back to the debt consultant.
        /// </summary>
        [Test, Description(@"This test will accept a proposal where only Life to inclusive. It ensures that the required work is done by the stored procedure and the expected
        rate overrides are created. It will also check that the proposal is accepted and the case is sent back to the debt consultant.")]
        public void AcceptProposalFixedPaymentInclusiveMonthlyServiceFee()
        {
            List<int> accounts = Service<IDebtCounsellingService>().GetAutomationDebtCounsellingTestCase(ProductEnum.NewVariableLoan, false);
            var accountKey = Service<IAccountService>().GetAccountWithOpenRelatedProducts(accounts, false, true);
            string id = Service<IAccountService>().GetIDNumbersForRoleOnAccount(accountKey, RoleTypeEnum.MainApplicant, GeneralStatusEnum.Active)[0];
            //create and process the case to the ManageProposal state
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant, idNumber: id, searchForCase: false);
            //insert the proposal
            Service<IProposalService>().InsertActiveProposal(base.TestCase.DebtCounsellingKey, 1, TestUsers.DebtCounsellingConsultant, 0, 0, 1);
            //send for approval
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.SendProposalForApprovalWithExistingProposal, base.TestCase.DebtCounsellingKey);
            base.LoadCase(WorkflowStates.DebtCounsellingWF.DecisiononProposal);
            AcceptProposal(ReasonDescription.ProposalAcceptance);
            //DebtCounsellingFixedPaymentInclusiveLife
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_22, FinancialAdjustmentStatusEnum.Inactive, true);
            //DebtCounsellingFixedRate
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_FixedRateAdjustment, FinancialAdjustmentStatusEnum.Inactive, true);
            //DebtCounsellingDiscountRate
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_InterestRateAdjustment, FinancialAdjustmentStatusEnum.Inactive, true);
        }

        /// <summary>
        /// This test will accept a proposal where only HOC to inclusive. It ensures that the required work is done by the stored procedure and the expected
        /// rate overrides are created. It will also check that the proposal is accepted and the case is sent back to the debt consultant.
        /// </summary>
        [Test, Description(@"This test will accept a proposal where HOC and MonthlyServiceFee to inclusive. It ensures that the required work is done by the stored procedure and the expected
        rate overrides are created. It will also check that the proposal is accepted and the case is sent back to the debt consultant.")]
        public void AcceptProposalFixedPaymentInclusiveHOCAndMonthlyServiceFee()
        {
            List<int> accounts = Service<IDebtCounsellingService>().GetAutomationDebtCounsellingTestCase(ProductEnum.NewVariableLoan, false);
            var accountKey = Service<IAccountService>().GetAccountWithOpenRelatedProducts(accounts, true, false);
            string id = Service<IAccountService>().GetIDNumbersForRoleOnAccount(accountKey, RoleTypeEnum.MainApplicant, GeneralStatusEnum.Active)[0];
            //create and process the case to the ManageProposal state
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant, idNumber: id, searchForCase: false);
            //insert the proposal
            Service<IProposalService>().InsertActiveProposal(base.TestCase.DebtCounsellingKey, 1, TestUsers.DebtCounsellingConsultant, 1, 0, 1);
            //send for approval
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.SendProposalForApprovalWithExistingProposal, base.TestCase.DebtCounsellingKey);
            base.LoadCase(WorkflowStates.DebtCounsellingWF.DecisiononProposal);
            AcceptProposal(ReasonDescription.ProposalAcceptance);
            //DebtCounsellingFixedPaymentInclusiveHOC
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_23, FinancialAdjustmentStatusEnum.Inactive, true);
            //DebtCounsellingFixedRate
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_FixedRateAdjustment, FinancialAdjustmentStatusEnum.Inactive, true);
            //DebtCounsellingDiscountRate
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_InterestRateAdjustment, FinancialAdjustmentStatusEnum.Inactive, true);
        }

        /// <summary>
        /// This test will accept a proposal where Life and MonthlyServiceFee are both inclusive. It ensures that the required work is done by the stored procedure and the expected
        /// rate overrides are created. It will also check that the proposal is accepted and the case is sent back to the debt consultant.
        /// </summary>
        [Test, Description(@"This test will accept a proposal where only Life to inclusive. It ensures that the required work is done by the stored procedure and the expected
        rate overrides are created. It will also check that the proposal is accepted and the case is sent back to the debt consultant.")]
        public void AcceptProposalFixedPaymentInclusiveLifeAndMonthlyServiceFee()
        {
            List<int> accounts = Service<IDebtCounsellingService>().GetAutomationDebtCounsellingTestCase(ProductEnum.NewVariableLoan, false);
            var accountKey = Service<IAccountService>().GetAccountWithOpenRelatedProducts(accounts, false, true);
            string id = Service<IAccountService>().GetIDNumbersForRoleOnAccount(accountKey, RoleTypeEnum.MainApplicant, GeneralStatusEnum.Active)[0];
            //create and process the case to the ManageProposal state
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant, idNumber: id, searchForCase: false);
            //insert the proposal
            Service<IProposalService>().InsertActiveProposal(base.TestCase.DebtCounsellingKey, 1, TestUsers.DebtCounsellingConsultant, 0, 1, 1);
            //send for approval
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.SendProposalForApprovalWithExistingProposal, base.TestCase.DebtCounsellingKey);
            base.LoadCase(WorkflowStates.DebtCounsellingWF.DecisiononProposal);
            AcceptProposal(ReasonDescription.ProposalAcceptance);
            //DebtCounsellingFixedPaymentInclusiveLife
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_PaymentAdjustment_24, FinancialAdjustmentStatusEnum.Inactive, true);
            //DebtCounsellingFixedRate
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_FixedRateAdjustment, FinancialAdjustmentStatusEnum.Inactive, true);
            //DebtCounsellingDiscountRate
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(accountKey, FinancialAdjustmentTypeSourceEnum.DebtCounselling_InterestRateAdjustment, FinancialAdjustmentStatusEnum.Inactive, true);
        }

        #endregion AcceptProposalRateOverrideTests

        private void AcceptProposal(string reasonDescription)
        {
            //check for previous acceptance
            bool previousApproval = Service<IStageTransitionService>().CheckIfTransitionExists(base.TestCase.DebtCounsellingKey, (int)StageDefinitionStageDefinitionGroupEnum.DebtCounselling_AcceptProposal);

            //get the user to be assigned
            string nextADUserToAssign = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                Workflows.DebtCounselling, DebtCounsellingLoadBalanceStates.consultantAssignmentExclusionStates, false, base.TestCase.DebtCounsellingKey);

            //get the account and the financial adjustments if they exist
            var acc = Service<IAccountService>().GetAccountByKey(base.TestCase.AccountKey);
            var previousProduct = acc.ProductKey;
            List<FinancialAdjustmentTypeSourceEnum> finAdjustments = Service<IFinancialAdjustmentService>().GetAccountFinancialAdjustmentsByStatus(FinancialAdjustmentStatusEnum.Active, base.TestCase.AccountKey,
                FinancialAdjustmentTypeSourceEnum.InterestOnly_InterestOnly,
                FinancialAdjustmentTypeSourceEnum.Staff_InterestRateAdjustment,
                FinancialAdjustmentTypeSourceEnum.DefendingCancellation_InterestRateAdjustment,
                FinancialAdjustmentTypeSourceEnum.SuspendedInterest_ReversalProvision_NonPerformingLoans
                );
            bool requiresOptOutBasedOnAdjustments = finAdjustments.Where<FinancialAdjustmentTypeSourceEnum>(
                finAdjustment => finAdjustment == FinancialAdjustmentTypeSourceEnum.InterestOnly_InterestOnly || finAdjustment == FinancialAdjustmentTypeSourceEnum.Staff_InterestRateAdjustment).Any();
            int proposalKey = Service<IProposalService>().GetProposalKeyByStatusAndAcceptedStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Active, ProposalAcceptedEnum.False,
                ProposalTypeEnum.Proposal);

            //check for the updated remaining term
            DateTime proposalEndDate = Service<IProposalService>().GetProposalEndDate(proposalKey);
            int remainingTerm = Service<IDebtCounsellingService>().GetDebtCounsellingRemainingTerm(base.TestCase.DebtCounsellingKey, proposalEndDate);

            //we need the data for the snapshot
            QueryResults snapshotData = null;
            if (!previousApproval)
                snapshotData = Service<IDebtCounsellingService>().GetAccountSnapShot(base.TestCase.DebtCounsellingKey);

            //start the action
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.Accept);
            base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(reasonDescription, ButtonTypeEnum.Submit);

            //Assertions
            //we need the accepted proposal key
            proposalKey = Service<IProposalService>().GetProposalKeyByStatusAndAcceptedStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Active, ProposalAcceptedEnum.True,
                ProposalTypeEnum.Proposal);

            //check which cases require opt out
            if ((acc.ProductKey == ProductEnum.VariFixLoan || acc.ProductKey == ProductEnum.SuperLo || acc.ProductKey == ProductEnum.Edge || acc.ProductKey == ProductEnum.DefendingDiscountRate) || requiresOptOutBasedOnAdjustments)
            {
                Service<IX2WorkflowService>().WaitForX2State(base.TestCase.DebtCounsellingKey, Workflows.DebtCounselling, WorkflowStates.DebtCounsellingWF.SendLoanAgreements);
                DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.SendLoanAgreements);
                StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_OptOutRequired);
                //account should now be opted out
                acc = Service<IAccountService>().GetAccountByKey(base.TestCase.AccountKey);
                Assert.AreEqual((int)ProductEnum.NewVariableLoan, (int)acc.ProductKey, "Account not opted into New Variable Loan product. AccountKey: {0}, previous ProductKey: {1}", acc.AccountKey, previousProduct);
            }
            else
            {
                Service<IX2WorkflowService>().WaitForX2State(base.TestCase.DebtCounsellingKey, Workflows.DebtCounselling, WorkflowStates.DebtCounsellingWF.AcceptedProposal);
                DebtCounsellingAssertions.AssertProposalAcceptance(base.TestCase.DebtCounsellingKey, base.TestCase.AccountKey, proposalKey, remainingTerm);
            }
            //if there were any of our rate overrides against the account then check that they have been made inactive
            if (finAdjustments.Count > 0)
            {
                foreach (var fin in finAdjustments)
                {
                    FinancialAdjustmentAssertions.AssertFinancialAdjustment(base.TestCase.AccountKey, fin, FinancialAdjustmentStatusEnum.Canceled, true);
                }
            }
            //check for stage transition
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_AcceptProposal);
            //check assignment
            WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(base.TestCase.DebtCounsellingKey, WorkflowRoleTypeEnum.DebtCounsellingConsultantD, nextADUserToAssign);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(Service<IX2WorkflowService>().GetInstanceIDByDebtCounsellingKey(base.TestCase.DebtCounsellingKey),
                nextADUserToAssign, WorkflowRoleTypeEnum.DebtCounsellingConsultantD, true, true);
            //proposal is now accepted with reason stored against it
            ReasonAssertions.AssertReason(reasonDescription, ReasonType.ProposalAccepted, proposalKey, GenericKeyTypeEnum.Proposal_ProposalKey, true);
            //Assert SnapShot Data
            if (snapshotData != null)
                DebtCounsellingAssertions.AssertDebtCounsellingAccountSnapShot(base.TestCase.DebtCounsellingKey, snapshotData);
        }
    }
}