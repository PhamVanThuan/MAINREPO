using BuildingBlocks.Services.Contracts;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LoanServicing
{
    public class DisbursementRollback : DisbursementRollbackControls
    {
        private readonly IWatiNService watinService;

        public DisbursementRollback()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Selects a loan transaction by its value from the Loan Transaction Grid in the Rollback Disbursement screen
        /// and then rolls back the disbursement by clicking the Rollback button
        /// </summary>
        /// <param name="b"></param>
        /// <param name="loanTransactionGridValue">Value of the Loan Transaction to be rolled back.</param>
        public void RollbackDisbursement(string loanTransactionGridValue)
        {
            base.gridSelectLoanDisbursementRecord(loanTransactionGridValue).Click();
            watinService.HandleConfirmationPopup(base.btnRollback);
        }
    }
}