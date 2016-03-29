using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;
using System.Collections.Generic;
using WatiN.Core;
using WatiN.Core.Logging;

namespace BuildingBlocks.Presenters.LoanServicing.CATSDisbursement
{
    public class CATSDisbursementRollback : CATSDisbursementRollbackControls
    {
        private readonly IWatiNService watinService;

        public CATSDisbursementRollback()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// On the CATSDisbursementRollback view select a transaction from the Loan Transactions grid and perform the Rollback action
        /// </summary>
        public void RollbackDisbursement(Automation.DataModels.Disbursement disbursement)
        {
            base.LoanTransactionRow(
                disbursement.DisbursementLoanTransaction.FinancialTransactionKey).Click();
            ClickButton(ButtonTypeEnum.Rollback);
        }

        /// <summary>
        /// Click the specified button on the CATSDisbursementRollback view
        /// </summary>
        /// <param name="button">ButtonType</param>
        public void ClickButton(ButtonTypeEnum button)
        {
            switch (button)
            {
                case ButtonTypeEnum.Rollback:
                    watinService.HandleConfirmationPopup(base.btnRollback);
                    break;

                case ButtonTypeEnum.Cancel:
                    base.btnCancel.Click();
                    break;

                default:
                    break;
            }
        }

        #region Validation

        /// <summary>
        /// Assert the expected controls exist on the CATSDisbursementRollback view
        /// </summary>
        public void AssertCATSDisbursementRollbackControlsExist()
        {
            var enabledAddControls = new List<Element>() {
                    base.btnCancel
                };

            Assertions.WatiNAssertions.AssertFieldsExist(enabledAddControls);
            Assertions.WatiNAssertions.AssertFieldsAreEnabled(enabledAddControls);
        }

        /// <summary>
        /// Assert that a record exists in the Disbursement Transactions Grid on the CATSDisbursementRollback view
        /// </summary>
        /// <param name="disbursement"></param>
        public void AssertRecordExistsInDisbursementTransactionsGrid(Automation.DataModels.Disbursement disbursement)
        {
            Logger.LogAction(@"Asserting Disbursement Bank Account Details displayed in grid");
            var row = base.DisbursementTransactionsRow(
                disbursement.PreparedDate,
                disbursement.ACBBankDescription,
                disbursement.ACBBranchCode,
                disbursement.ACBBranchDescription,
                disbursement.ACBTypeDescription,
                disbursement.AccountNumber,
                disbursement.AccountName,
                disbursement.Amount
                );
            Assert.False(row == null, @"No matching row was found in the Disbursement Transactions grid");
        }

        /// <summary>
        /// Assert that a record exists in the Loan Transactions Grid on the CATSDisbursementRollback view
        /// </summary>
        /// <param name="disbursement"></param>
        public void AssertRecordExistsInLoanTransactionsGrid(Automation.DataModels.Disbursement disbursement)
        {
            Logger.LogAction(@"Asserting record displayed in Loan Transactions grid");
            var row = base.LoanTransactionRow(
                disbursement.DisbursementLoanTransaction.LoanTransaction.FinancialTransactionKey
                );
            Assert.False(row == null, @"No matching row was found in the loan Transactions grid");
        }

        #endregion Validation
    }
}