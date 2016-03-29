using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.FLOBO.PersonalLoan;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace PersonalLoansTests.LoanServicing
{
    [RequiresSTA]
    public class ClosePersonalLoanAccountTests : PersonalLoansWorkflowTestBase<PostTransaction>
    {
        #region Test Setup/Teardown

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            base.Service<IWatiNService>().KillAllIEProcesses();
        }

        #endregion Test Setup/Teardown

        #region Tests

        /// <summary>
        /// This test verifies that when closing a Personal Loan Account which has a Zero balance and a Credit Life policy,
        /// * The account status is updated to closed.
        /// * The SAHL Credit Life Policy is closed automatically.
        /// </summary>
        [Test, Description(@"This test verifies that when closing a Personal Loan Account which has a Credit Life policy, the account status is updated to closed and the SAHL Credit Life Policy is closed automatically")]
        public void CloseAccountWithAZeroBalanceAndACreditLifePolicy()
        {
            // Find a personal loan account which has a Credit Life Policy
            base.FindPersonalLoanAccountAndLoadIntoLoanServicing(true, TestUsers.PersonalLoanClientServiceUser);
            Service<IDisbursementService>().UpdateReadyForDisbursementToDisbursed(this.GenericKey);
            var creditLife = base.Service<IAccountService>().GetOpenRelatedAccountsByProductKey(this.GenericKey, ProductEnum.SAHLCreditProtectionPlan).Rows(0).Column("AccountKey").GetValueAs<int>();

            // Get the outstanding balance of the Loan and the set the current balance to zero
            double currentBalance = base.Service<IAccountService>().GetCurrentPersonalLoanBalance(this.GenericKey);
            var financialService = Service<IAccountService>().GetOpenFinancialServiceRecordByType(this.GenericKey, FinancialServiceTypeEnum.PersonalLoan);
            base.FinancialServiceKey = financialService.Rows(0).Column("FinancialServiceKey").GetValueAs<int>();
            base.Service<ILoanTransactionService>().pProcessTran(base.FinancialServiceKey, TransactionTypeEnum.PrePayment320, (decimal)currentBalance, "Personal Loan Test", @"SAHL\TestUser");

            // Click the Close Personal Loan account and then click the confirm button
            base.Browser.Navigate<PersonalLoanNode>().ClickClosePersonalLoanAccount();
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);

            // Assert that the Account status is updated to closed.
            AccountAssertions.AssertAccountStatus(base.GenericKey, AccountStatusEnum.Closed);

            // TODO Assert that the Credit Life policy acc is closed.
            AccountAssertions.AssertAccountStatus(creditLife, AccountStatusEnum.Closed);
        }

        /// <summary>
        /// This test verifies that when closing a Personal Loan Account which has a zero balance and no Credit Life policy,
        /// the account status is updated to closed.
        /// </summary>
        [Test, Description(@"This test verifies that when closing a Personal Loan Account which has a zero balance and no Credit Life policy, the account status is updated to closed.")]
        public void CloseAccountWithAZeroBalanceAndNoCreditLifePolicy()
        {
            // Find a personal loan account which has NO Credit Life Policy
            base.FindPersonalLoanAccountAndLoadIntoLoanServicing(false, TestUsers.PersonalLoanClientServiceUser);
            Service<IDisbursementService>().UpdateReadyForDisbursementToDisbursed(this.GenericKey);
            // Get the outstanding balance of the Loan and the set the current balance to zero
            double currentBalance = base.Service<IAccountService>().GetCurrentPersonalLoanBalance(this.GenericKey);
            var financialService = Service<IAccountService>().GetOpenFinancialServiceRecordByType(this.GenericKey, FinancialServiceTypeEnum.PersonalLoan);
            base.Service<ILoanTransactionService>().pProcessTran(Convert.ToInt32(financialService.Rows(0).Column("FinancialServiceKey").Value), TransactionTypeEnum.PrePayment320, (decimal)currentBalance, "Personal Loan Test", @"SAHL\TestUser");

            // Click the Close Personal Loan account and then click the confirm button
            base.Browser.Navigate<PersonalLoanNode>().ClickClosePersonalLoanAccount();
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);

            // Assert that the Account status is updated to closed.
            AccountAssertions.AssertAccountStatus(base.GenericKey, AccountStatusEnum.Closed);
        }

        /// <summary>
        /// This test verifies that a Personal Loans Loan Servicing User cannot close an account that does not have zero balance.
        /// </summary>
        [Test, Description(@"This test verifies that a Personal Loans Loan Servicing User cannot close an account that does not have zero balance.")]
        public void CloseAccountWithoutAZeroBalance()
        {
            // Find an open personal loan account which has no Credit Life Policy
            base.FindPersonalLoanAccountAndLoadIntoLoanServicing(true, TestUsers.PersonalLoanClientServiceUser);
            Service<IDisbursementService>().UpdateReadyForDisbursementToDisbursed(this.GenericKey);
            // Click the Close Personal Loan account and then click the confirm button
            base.Browser.Navigate<PersonalLoanNode>().ClickClosePersonalLoanAccount();
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);

            // Assert that only accounts with a zero balance can be closed.
            var msg = string.Format("Balance is not 0");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains(msg);

            AccountAssertions.AssertAccountStatus(base.GenericKey, AccountStatusEnum.Open);
        }

        #endregion Tests
    }
}