using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Events;
using System.Linq;


namespace SAHL.Web.Views.Common.Presenters
{
    public class LoanFinancialServiceSummaryQuickCash : SAHLCommonBasePresenter<ILoanFinancialServiceSummary>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LoanFinancialServiceSummaryQuickCash(ILoanFinancialServiceSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.LoyaltyButtonClicked += new EventHandler(_view_LoyaltyButtonClicked);
            _view.AmortisingInstallmentVisible = false;
            _view.LoyaltyButtonVisible = false;

            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (cboNode != null)
            {
                IFinancialServiceRepository finserviceRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
                IFinancialService financialService = finserviceRepo.GetFinancialServiceByKey(cboNode.GenericKey);
                if (financialService != null)
                {

                    IMortgageLoanAccount mortgageLoanAccount = financialService.Account as IMortgageLoanAccount;
                    IMortgageLoan mortgageLoan = null;

                    if (mortgageLoanAccount != null)
                    {
                        if (mortgageLoanAccount.UnsecuredMortgageLoans != null && mortgageLoanAccount.UnsecuredMortgageLoans.Count > 0)
                        {
                            foreach (IMortgageLoan _ml in mortgageLoanAccount.UnsecuredMortgageLoans)
                            {
                                if (_ml.MortgageLoanPurpose.Key == (int)MortgageLoanPurposes.QuickCash)
                                {
                                    mortgageLoan = _ml;
                                    break;
                                }
                            }
                        }
                        else
                            return;

                        if (mortgageLoan != null)
                        {
                            if (mortgageLoan.HasInterestOnly())
                            {
                                double dCurrentBalance = mortgageLoan.CurrentBalance;
                                double dInterestRate = mortgageLoan.InterestRate;
                                double dRemainingTerm = mortgageLoan.RemainingInstallments;
                                double AmortisingInstallmentValue = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(dCurrentBalance, dInterestRate, dRemainingTerm, false);
                                if (AmortisingInstallmentValue > 0.01)
                                {
                                    _view.AmortisingInstallmentVisible = true;
                                    _view.AmortisationInstalment = AmortisingInstallmentValue;
                                }
                            }

                            
                            IFinancialService finService = mortgageLoan as IFinancialService;

                            double currenttoDate = 0;
                            double totalforMonth = 0;

                            if(finService.AccountStatus.Key == (int)AccountStatuses.Open
                            || finService.AccountStatus.Key == (int)AccountStatuses.Dormant
                            || finService.AccountStatus.Key == (int)AccountStatuses.Locked)
                            mortgageLoanAccount.CalculateInterest(financialService.Key, out currenttoDate, out totalforMonth);
                            
                            _view.InterestCurrenttoDate = currenttoDate;
                            _view.InterestTotalforMonth = totalforMonth;

                            List<int> tranTypes = new List<int>();
                            tranTypes.Add((int)SAHL.Common.Globals.TransactionTypes.MonthlyInterestDebit); // 210
                            tranTypes.Add((int)SAHL.Common.Globals.TransactionTypes.MonthlyInterestDebitCorrection); // 1210
                            DataTable dtLoanTransactions = finService.GetTransactions(_view.Messages, tranTypes);
                            double previousMonthInterest = 0;
                            for (int i = 0; i < dtLoanTransactions.Rows.Count; i++)
                            {
                                DataRow transactionRow = dtLoanTransactions.Rows[i];
                                if (Convert.ToDateTime(transactionRow["LoanTransactionEffectiveDate"]) == (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)))
                                    previousMonthInterest += Convert.ToDouble(transactionRow["LoanTransactionAmount"]);
                            }
                            _view.InterestPreviousMonth = previousMonthInterest;

                            if (mortgageLoan.Account.Product.Key == Convert.ToInt32(Products.SuperLo))
                                _view.LoyaltyButtonVisible = true;

                            _view.BindFinancialServiceSummaryData(mortgageLoan);
                            if (mortgageLoan.FinancialAdjustments != null)
                            {
                                // Sort the FADJ's    
                                IList<IFinancialAdjustment> financialAdjustmentsSorted = mortgageLoan.FinancialAdjustments.OrderBy(x => x.FinancialAdjustmentStatus.Key).ThenBy(y => y.FromDate).ToList<IFinancialAdjustment>();
                                // Bind the FADJ's
                                _view.BindFinancialAdjustmentGrid(financialAdjustmentsSorted);
                            }

                        }
                    }
                }
            }
        }


        void _view_LoyaltyButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("SuperLoLoyaltyInfo");
        }
    }
}
