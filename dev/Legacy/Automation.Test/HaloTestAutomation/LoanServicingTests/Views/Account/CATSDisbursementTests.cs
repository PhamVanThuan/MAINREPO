using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing.CATSDisbursement;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using WatiN.Core.Logging;

namespace LoanServicingTests.Views.Account
{
    [RequiresSTA]
    public class CATSDisbursementTests : TestBase<CATSDisbursementAdd>
    {
        #region SetupTearDown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            // open browser with test user
            base.Browser = new TestBrowser(TestUsers.HaloUser, TestUsers.Password);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            //remove any nodes from CBO
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            // navigate to ClientSuperSearch
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().Menu();
        }

        #endregion SetupTearDown

        #region View Tests

        /// <summary>
        /// Check that the CATSDisbursementDisplay view displays correctly when no pending disbursement records exist
        /// </summary>
        [Test]
        public void CATSDisbursementDisplay_Navigation_NoDisbursementRecords()
        {
            var account = Service<IDisbursementService>().GetRandomOpenAccountWithNoDisbursementsInStatus(DisbursementStatusEnum.Pending);
            NavigateToCATSDisbursmentNode(account.AccountKey, NodeTypeEnum.View);

            //Assertions
            base.Browser.Page<BasePageAssertions>().AssertFrameContainsText("There are no disbursement records.");
        }

        /// <summary>
        /// Check that the CATSDisbursementDisplay view displays correctly when pending disbursement records exist
        /// </summary>
        [Test]
        public void CATSDisbursementDisplay_Navigation_PendingDisbursementRecords()
        {
            var disbursement = Service<IDisbursementService>().GetRandomOpenAccountWithDisbursements(DisbursementTransactionTypeEnum.CancellationRefund, DisbursementStatusEnum.Pending);
            if (disbursement == null)
            {
                Logger.LogAction(@"No test data exist.  Attempting to create test data");
                CancellationRefund_BalanceEqualsZero_NewVariableLoan();
                disbursement = Service<IDisbursementService>().GetRandomOpenAccountWithDisbursements(DisbursementTransactionTypeEnum.CancellationRefund, DisbursementStatusEnum.Pending);
            }
            NavigateToCATSDisbursmentNode(disbursement.AccountKey, NodeTypeEnum.View);
            base.Browser.Page<CATSDisbursementBase>().AssertRecordExistsInDisbursementBankAccountDetailsGrid(disbursement);
        }

        #endregion View Tests

        #region Add Tests

        /// <summary>
        /// Check that the CATSDisbursementAdd view displays correctly when no pending disbursement records exist
        /// </summary>
        [Test]
        public void CATSDisbursementAdd_Navigation_NoDisbursementRecords()
        {
            var account = Service<IDisbursementService>().GetRandomOpenAccountWithNoDisbursementsInStatus(DisbursementStatusEnum.Pending);
            NavigateToCATSDisbursmentNode(account.AccountKey, NodeTypeEnum.Add);

            //Asserions
            base.Browser.Page<BasePageAssertions>().AssertFrameContainsText("There are no disbursement records.");
            base.View.AssertCATSDisbursementAddControlsExist_NoDisbursementRecords();
            var disbursementTypes = new List<string>() {
                    DisbursementTransactionTypes.CancellationRefund,
                    DisbursementTransactionTypes.ReAdvance,
                    DisbursementTransactionTypes.Refund,
                    DisbursementTransactionTypes.CAP2ReAdvance
                };
            base.View.AssertDisbursementTypeOptions(disbursementTypes);
        }

        /// <summary>
        /// Check that the CATSDisbursementAdd view displays correctly when pending disbursement records exist
        /// </summary>
        [Test]
        public void CATSDisbursementAdd_Navigation_PendingDisbursementRecords()
        {
            var disbursement = Service<IDisbursementService>().GetRandomOpenAccountWithDisbursements(DisbursementTransactionTypeEnum.CancellationRefund, DisbursementStatusEnum.Pending);
            if (disbursement == null)
            {
                Logger.LogAction(@"No test data exist.  Attempting to create test data");
                CancellationRefund_BalanceEqualsZero_NewVariableLoan();
                disbursement = Service<IDisbursementService>().GetRandomOpenAccountWithDisbursements(DisbursementTransactionTypeEnum.CancellationRefund, DisbursementStatusEnum.Pending);
            }
            NavigateToCATSDisbursmentNode(disbursement.AccountKey, NodeTypeEnum.Add);
            //Assertions
            base.Browser.Page<CATSDisbursementBase>().AssertRecordExistsInDisbursementBankAccountDetailsGrid(disbursement);
            base.View.AssertCATSDisbursementAddControlsExist_PendingDisbursementRecords();
        }

        /// <summary>
        /// Test mandatory fields on the CATSDisbursementAdd view
        /// </summary>
        [Test]
        public void CATSDisbursementAdd_MandatoryFields()
        {
            var account = Service<IDisbursementService>().GetRandomOpenAccountWithNoDisbursementsInStatus(DisbursementStatusEnum.Pending);
            NavigateToCATSDisbursmentNode(account.AccountKey, NodeTypeEnum.Add);

            base.View.AddDisbursement(DisbursementTransactionTypes.CancellationRefund, 2, "Test", -1, 1, ButtonTypeEnum.Add);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please select Bank Account.");

            base.View.AddDisbursement(0, 2, "Test", 1, 1, ButtonTypeEnum.Add);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please select the Disbursement Type.");
        }

        /// <summary>
        /// Ensure that it is not possble to disburse a zero amount
        /// </summary>
        [Test]
        public void DisbursementGreaterThanZeroCheck()
        {
            var account = Service<IDisbursementService>().GetRandomOpenAccountWithNoDisbursementsInStatus(DisbursementStatusEnum.Pending);
            NavigateToCATSDisbursmentNode(account.AccountKey, NodeTypeEnum.Add);

            decimal disbursementAmount = 0;

            base.View.AddDisbursement(DisbursementTransactionTypes.CancellationRefund, disbursementAmount, "Test", 1, disbursementAmount, ButtonTypeEnum.Add);
            base.View.ClickButton(ButtonTypeEnum.Save);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Disbursement must be greater than zero and less than R 10 000 000");
        }

        /// <summary>
        /// Ensure that it is not possible to disburse and amount greater than 10000000
        /// </summary>
        [Test]
        [Ignore]
        public void DisbursementLessThan10000000Check()
        {
            var account = Service<IDisbursementService>().GetRandomOpenAccountWithNoDisbursementsInStatus(DisbursementStatusEnum.Pending);
            NavigateToCATSDisbursmentNode(account.AccountKey, NodeTypeEnum.Add);

            decimal disbursementAmount = 10000000;

            base.View.AddDisbursement(DisbursementTransactionTypes.Refund, disbursementAmount, "Test", 1, disbursementAmount, ButtonTypeEnum.Add);
            base.View.ClickButton(ButtonTypeEnum.Save);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Disbursement must be greater than zero and less than R 10 000 000");
        }

        /// <summary>
        /// Ensure that it is not possible to disburse a Cancellation Refund that will result in the loan having a possitive balance
        /// </summary>
        [Test]
        public void CancellationRefund_BalanceGreaterThanZeroCheck_NewVariableLoan()
        {
            var account = base.Service<IAccountService>().GetRandomAccountWithNegativeBalance(ProductEnum.NewVariableLoan, AccountStatusEnum.Open);
            NavigateToCATSDisbursmentNode(account.AccountKey, NodeTypeEnum.Add);

            decimal disbursementAmount = Math.Abs(account.CurrentBalance) + 1;

            base.View.AddDisbursement(DisbursementTransactionTypes.CancellationRefund, disbursementAmount, "Test", 1, disbursementAmount, ButtonTypeEnum.Add);
            base.View.ClickButton(ButtonTypeEnum.Save);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Cancellation Refund may not be saved for this account, resulting balance will be > R 0.00");
        }

        /// <summary>
        /// Ensure that it is not possible to disburse a Cancellation Refund that will result in the loan having a possitive balance
        /// </summary>
        [Test]
        public void CancellationRefund_BalanceGreaterThanZeroCheck_VariFixLoan()
        {
            var account = base.Service<IAccountService>().GetRandomAccountWithNegativeBalance(ProductEnum.VariFixLoan, AccountStatusEnum.Open);
            NavigateToCATSDisbursmentNode(account.AccountKey, NodeTypeEnum.Add);

            decimal disbursementAmount = Math.Abs(account.CurrentBalance) + 1;

            base.View.AddDisbursement(DisbursementTransactionTypes.CancellationRefund, disbursementAmount, "Test", 1, disbursementAmount, ButtonTypeEnum.Add);
            base.View.ClickButton(ButtonTypeEnum.Save);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Cancellation Refund may not be saved for this account, resulting balance will be > R 0.00");
        }

        /// <summary>
        /// Ensure that it is possible to disburse a Cancellation Refund that results in the loan having a zero balance
        /// </summary>
        [Test]
        public void CancellationRefund_BalanceEqualsZero_NewVariableLoan()
        {
            var account = base.Service<IAccountService>().GetRandomAccountWithNegativeBalance(ProductEnum.NewVariableLoan, AccountStatusEnum.Open);
            NavigateToCATSDisbursmentNode(account.AccountKey, NodeTypeEnum.Add);

            decimal disbursementAmount = Math.Abs(account.CurrentBalance);

            base.View.AddDisbursement(DisbursementTransactionTypes.CancellationRefund, disbursementAmount, "Test", 1, disbursementAmount, ButtonTypeEnum.Add);
            base.View.ClickButton(ButtonTypeEnum.Save);

            DisbursementAssertions.AssertDisbursementAmount(account.AccountKey, DisbursementStatusEnum.Pending, DisbursementTransactionTypeEnum.CancellationRefund, disbursementAmount);
            //browser.Page<Views.CATSDisbursementBase>().AssertRecordExistsInDisbursementBankAccountDetailsGrid(disbursement);
            base.View.AssertCATSDisbursementAddControlsExist_PendingDisbursementRecords();
        }

        /// <summary>
        /// Ensure that it is possible to disburse a Cancellation Refund that results in the loan having a zero balance
        /// </summary>
        [Test]
        public void CancellationRefundBalanceEqualsZeroVariFixLoan()
        {
            var account = base.Service<IAccountService>().GetRandomAccountWithNegativeBalance(ProductEnum.VariFixLoan, AccountStatusEnum.Open);
            NavigateToCATSDisbursmentNode(account.AccountKey, NodeTypeEnum.Add);

            decimal disbursementAmount = Math.Abs(account.CurrentBalance);

            base.View.AddDisbursement(DisbursementTransactionTypes.CancellationRefund, disbursementAmount, "Test", 1, disbursementAmount, ButtonTypeEnum.Add);
            base.View.ClickButton(ButtonTypeEnum.Save);

            DisbursementAssertions.AssertDisbursementAmount(account.AccountKey, DisbursementStatusEnum.Pending, DisbursementTransactionTypeEnum.CancellationRefund, disbursementAmount);
            //browser.Page<Views.CATSDisbursementBase>().AssertRecordExistsInDisbursementBankAccountDetailsGrid(disbursement);
            base.View.AssertCATSDisbursementAddControlsExist_PendingDisbursementRecords();
        }

        /// <summary>
        /// Ensure that it is not possible to disburse a ReAdvance if an application does not exist against the loan
        /// </summary>
        [Test]
        public void ReAdvance_NoOpenApplicationCheck()
        {
            var account = base.Service<IAccountService>().GetVariableLoanAccountByMainApplicantCount(1, 1, Common.Enums.AccountStatusEnum.Open);
            NavigateToCATSDisbursmentNode(account.AccountKey, NodeTypeEnum.Add);

            base.View.AddDisbursement(DisbursementTransactionTypes.ReAdvance, 1, "Test", 1, 1, ButtonTypeEnum.Add);
            base.View.ClickButton(ButtonTypeEnum.Save);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("No open application exists for this Disbursement.");
        }

        /// <summary>
        /// Ensure that it is not possible to disburse a CAP2Readvance if no CAP offers awaiting Readvance exist against the loan
        /// </summary>
        [Test]
        public void CAP2ReadvanceCAPOffersAwaitingReadvanceCheck()
        {
            var account = base.Service<IAccountService>().GetVariableLoanAccountByMainApplicantCount(1, 1, Common.Enums.AccountStatusEnum.Open);
            NavigateToCATSDisbursmentNode(account.AccountKey, NodeTypeEnum.Add);

            base.View.SelectDisbursementType(DisbursementTransactionTypes.CAP2ReAdvance);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("No Cap Offers Awaiting Readvance Found !.");
        }

        /// <summary>
        /// Ensure that it is possible to post a Cancellation Refund disbursement
        /// </summary>
        [Test]
        public void CancellationRefund_Post()
        {
            var accountDisbursement = Service<IDisbursementService>().GetRandomOpenAccountWithDisbursements(DisbursementTransactionTypeEnum.CancellationRefund, DisbursementStatusEnum.Pending);
            if (accountDisbursement == null)
            {
                CancellationRefund_BalanceEqualsZero_NewVariableLoan();
                accountDisbursement = Service<IDisbursementService>().GetRandomOpenAccountWithDisbursements(DisbursementTransactionTypeEnum.CancellationRefund, DisbursementStatusEnum.Pending);
            }
            NavigateToCATSDisbursmentNode(accountDisbursement.AccountKey, NodeTypeEnum.Add);

            base.View.ClickButton(ButtonTypeEnum.Post);
            DisbursementAssertions.AssertDisbursementStatus(new List<int>() { accountDisbursement.DisbursementKey }, DisbursementStatusEnum.ReadyForDisbursement);
            TransactionAssertions.AssertLoanTransactionExists(accountDisbursement.AccountKey, TransactionTypeEnum.CancellationRefund);
        }

        #endregion Add Tests

        #region Delete Tests

        /// <summary>
        /// Verify that the CATSDisbursementDelete view displays correctly when no pending disbursement records exist
        /// </summary>
        [Test]
        public void CATSDisbursementDelete_Navigation_NoDisbursementRecords()
        {
            var account = Service<IDisbursementService>().GetRandomOpenAccountWithNoDisbursementsInStatus(DisbursementStatusEnum.Pending);
            NavigateToCATSDisbursmentNode(account.AccountKey, NodeTypeEnum.Delete);

            //Assertions
            base.Browser.Page<BasePageAssertions>().AssertFrameContainsText("There are no disbursement records.");
            base.Browser.Page<CATSDisbursementDelete>().AssertCATSDisbursementDeleteControlsExist_NoDisbursementRecords();
        }

        /// <summary>
        /// Verify that the CATSDisbursementDelete view displays correctly when pending disbursement records exist
        /// </summary>
        [Test]
        public void CATSDisbursementDelete_Navigation_PendingDisbursementRecord()
        {
            var disbursement = Service<IDisbursementService>().GetRandomOpenAccountWithDisbursements(DisbursementTransactionTypeEnum.CancellationRefund, DisbursementStatusEnum.Pending);
            if (disbursement == null)
            {
                Logger.LogAction(@"No test data exist.  Attempting to create test data");
                CancellationRefund_BalanceEqualsZero_NewVariableLoan();
                disbursement = Service<IDisbursementService>().GetRandomOpenAccountWithDisbursements(DisbursementTransactionTypeEnum.CancellationRefund, DisbursementStatusEnum.Pending);
            }
            NavigateToCATSDisbursmentNode(disbursement.AccountKey, NodeTypeEnum.Delete);

            //Assertions
            base.Browser.Page<CATSDisbursementBase>().AssertRecordExistsInDisbursementBankAccountDetailsGrid(disbursement);
            base.Browser.Page<CATSDisbursementDelete>().AssertCATSDisbursementDeleteControlsExist_PendingDisbursementRecords();
        }

        /// <summary>
        /// Ensure that it is possible to delete a pending CancellationRefund disbursement
        /// </summary>
        [Test]
        public void CancellationRefundDelete()
        {
            Automation.DataModels.Disbursement accountDisbursement = Service<IDisbursementService>().GetRandomOpenAccountWithDisbursements(DisbursementTransactionTypeEnum.CancellationRefund, DisbursementStatusEnum.Pending);
            if (accountDisbursement == null)
            {
                Logger.LogAction(@"No test data exist.  Attempting to create test data");
                CancellationRefund_BalanceEqualsZero_NewVariableLoan();
                accountDisbursement = Service<IDisbursementService>().GetRandomOpenAccountWithDisbursements(DisbursementTransactionTypeEnum.CancellationRefund, DisbursementStatusEnum.Pending);
            }
            NavigateToCATSDisbursmentNode(accountDisbursement.AccountKey, NodeTypeEnum.Delete);

            base.Browser.Page<CATSDisbursementDelete>().DeleteDisbursement();
            base.Browser.Page<CATSDisbursementDelete>().AssertDisbursementRecordDeleted(accountDisbursement.DisbursementKey);
        }

        #endregion Delete Tests

        #region Rollback Tests

        /// <summary>
        /// Verify that it is possible to navigate to the CATSDisbursementRollback view.
        /// Assert the expected controls exist on the CATSDisbursementRollback view
        /// </summary>
        [Test]
        public void CATSDisbursementRollback_Navigation()
        {
            Automation.DataModels.Disbursement disbursement = Service<IDisbursementService>().GetRandomOpenAccountWithDisbursementLoanTransactions(DisbursementTransactionTypeEnum.CancellationRefund, DisbursementStatusEnum.ReadyForDisbursement);
            if (disbursement == null)
            {
                Logger.LogAction(@"No test data exist.  Attempting to create test data");
                CancellationRefund_Post();
                disbursement = Service<IDisbursementService>().GetRandomOpenAccountWithDisbursementLoanTransactions(DisbursementTransactionTypeEnum.CancellationRefund, DisbursementStatusEnum.ReadyForDisbursement);
            }
            NavigateToCATSDisbursmentNode(disbursement.AccountKey, NodeTypeEnum.Rollback);

            //Asserions
            base.Browser.Page<CATSDisbursementRollback>().AssertRecordExistsInLoanTransactionsGrid(disbursement);
            base.Browser.Page<CATSDisbursementRollback>().AssertCATSDisbursementRollbackControlsExist();
        }

        /// <summary>
        /// Ensure it is possible to Rollback a Cancellation Refund transaction ready for disbursment
        /// </summary>
        [Test]
        public void CancellationRefund_Rollback_ReadyForDisbursement()
        {
            var disbursement = Service<IDisbursementService>().GetRandomOpenAccountWithDisbursementLoanTransactions(DisbursementTransactionTypeEnum.CancellationRefund, DisbursementStatusEnum.ReadyForDisbursement);
            if (disbursement == null)
            {
                Logger.LogAction(@"No test data exist.  Attempting to create test data");
                CancellationRefund_Post();
                disbursement = Service<IDisbursementService>().GetRandomOpenAccountWithDisbursementLoanTransactions(DisbursementTransactionTypeEnum.CancellationRefund, DisbursementStatusEnum.ReadyForDisbursement);
            }
            NavigateToCATSDisbursmentNode(disbursement.AccountKey, NodeTypeEnum.Rollback);

            base.Browser.Page<CATSDisbursementRollback>().RollbackDisbursement(disbursement);
            DisbursementAssertions.AssertDisbursementStatus(new List<int>() { disbursement.DisbursementKey }, DisbursementStatusEnum.RolledBack);
            TransactionAssertions.AssertLoanTransactionExists(disbursement.AccountKey, TransactionTypeEnum.CancellationRefundCorrection);
        }

        /// <summary>
        /// Ensure it is not possible to Rollback a disbursed Cancellation Refund transaction
        /// </summary>
        [Test]
        public void CancellationRefund_Rollback_Disbursed()
        {
            DateTime date = DateTime.Now;
            var disbursement = Service<IDisbursementService>().GetRandomOpenAccountWithDisbursementLoanTransactions(DisbursementTransactionTypeEnum.CancellationRefund, DisbursementStatusEnum.Disbursed);
            if (disbursement == null)
            {
                Logger.LogAction(@"No test data exist.  Attempting to create test data");
                CancellationRefund_Post();
                disbursement = Service<IDisbursementService>().GetRandomOpenAccountWithDisbursementLoanTransactions(DisbursementTransactionTypeEnum.CancellationRefund, DisbursementStatusEnum.Disbursed);
            }
            NavigateToCATSDisbursmentNode(disbursement.AccountKey, NodeTypeEnum.Rollback);

            base.Browser.Page<CATSDisbursementRollback>().RollbackDisbursement(disbursement);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("This item has already been disbursed and can never be rolled back.");
        }

        #endregion Rollback Tests

        #region Helpers

        /// <summary>
        /// Get an Open Account from the DB
        /// Search for the Account by AccountKey from the Client Super Search
        /// </summary>
        /// <param name="node">Add, Delete, View or Rollback </param>
        private void NavigateToCATSDisbursmentNode(int accountKey, NodeTypeEnum node)
        {
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().CATSDisbursement(node);
        }

        #endregion Helpers
    }
}