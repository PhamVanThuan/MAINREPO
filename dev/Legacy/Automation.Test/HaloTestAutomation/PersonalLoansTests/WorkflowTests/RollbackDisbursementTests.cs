using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    public class RollbackDisbursementTests : PersonalLoansWorkflowTestBase<WorkflowYesNo>
    {
        private TimeSpan cutoffTime = new TimeSpan(12, 30, 00);

        protected override void OnTestStart()
        {
            base.OnTestStart();
            if (Service<ICommonService>().IsTimeOver(cutoffTime))
                base.FailTest("Rollback Disbursment tests can only be run prior to cutoff time");
        }

        /// <summary>
        /// This test will find a case at the Disbursed stage and rollback the disbursement.  It will then ensure the the financial/arrear transactions have been
        /// removed, the disbursement status has been set to Rolled Back, the account status has set to Applciation and the case has moved back to the Disbursement
        /// state.
        /// </summary>
        [Test]
        public void RollbackPersonalLoanDisbursement()
        {
            var accountkey = RollbackDisbursement(DateTime.Now);
            base.Service<IX2WorkflowService>().WaitForX2State(base.GenericKey, Workflows.PersonalLoans, WorkflowStates.PersonalLoansWF.Disbursement);
            TransactionAssertions.AssertFinancialTransactionsDeleted(accountkey, FinancialServiceTypeEnum.PersonalLoan);
            TransactionAssertions.AssertArrearTransactionsDeleted(accountkey, FinancialServiceTypeEnum.PersonalLoan);
            DisbursementAssertions.AssertDisbursementExistsAtStatus(accountkey, DisbursementStatusEnum.RolledBack, DisbursementTransactionTypeEnum.Payment_NoInterest);
            base.Service<IStageTransitionService>().CheckIfTransitionExists(base.GenericKey, (int)StageDefinitionStageDefinitionGroupEnum.PersonalLoans_RollbackDisbursement);
            AccountAssertions.AssertAccountStatus(accountkey, AccountStatusEnum.Application);
            OfferAssertions.AssertOfferStatus(base.GenericKey, OfferStatusEnum.Open);
            X2Assertions.AssertCurrentX2State(base.InstanceID, WorkflowStates.PersonalLoansWF.Disbursement);
        }

        private int RollbackDisbursement(DateTime accountOpenDate)
        {
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.Disbursed, WorkflowRoleTypeEnum.PLSupervisorD);
            int accountkey = base.Service<IAccountService>().GetAccountByKey(base.personalLoanApplication.ReservedAccountKey).AccountKey;
            //Need to make sure that when disbursfunds posted the right transactions before testing that those transactions are rolled back.
            TransactionAssertions.AssertLoanTransactionExists(accountkey, FinancialServiceTypeEnum.PersonalLoan, TransactionTypeEnum.PersonalLoan);
            TransactionAssertions.AssertLoanTransactionExists(accountkey, FinancialServiceTypeEnum.PersonalLoan, TransactionTypeEnum.PersonalLoanInitiationFee);
            TransactionAssertions.AssertLoanTransactionExists(accountkey, FinancialServiceTypeEnum.PersonalLoan, TransactionTypeEnum.PersonalLoan);
            base.Service<IAccountService>().UpdateOpenDate(accountkey, accountOpenDate);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.RollbackDisbursement);
            base.View.ClickYes();
            return accountkey;
        }
    }
}