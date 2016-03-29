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
    /// <summary>
    /// 
    /// </summary>
    public class LoanFinancialServiceSummaryFixed : SAHLCommonBasePresenter<ILoanFinancialServiceSummary>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LoanFinancialServiceSummaryFixed(ILoanFinancialServiceSummary view, SAHLCommonBaseController controller)
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

            _view.AmortisingInstallmentVisible = false;
            _view.LoyaltyButtonVisible = false;
            _view.LoyaltyButtonClicked += new EventHandler(_view_LoyaltyButtonClicked);

            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (cboNode != null)
            {
                IFinancialServiceRepository finserviceRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
                IFinancialService financialService = finserviceRepo.GetFinancialServiceByKey(cboNode.GenericKey);
                if (financialService != null)
                {
                    IAccountVariFixLoan varifixLoanAccount = financialService.Account as IAccountVariFixLoan;
                    if (varifixLoanAccount != null)
                    {
                        IMortgageLoan mortgageLoan = varifixLoanAccount.FixedSecuredMortgageLoan;
                        if (mortgageLoan != null)
                        {
                            if (mortgageLoan.HasInterestOnly())
                            {
                                double dCurrentBalance = mortgageLoan.CurrentBalance;
                                double dInterestRate = mortgageLoan.InterestRate;
                                double dRemainingTerm = mortgageLoan.RemainingInstallments;
                                double amortisingInstallmentValue = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(dCurrentBalance, dInterestRate, dRemainingTerm, false);
                                if (amortisingInstallmentValue > 0.01)
                                {
                                    _view.AmortisingInstallmentVisible = true;
                                    _view.AmortisationInstalment = amortisingInstallmentValue;
                                }
                            }

                            IFinancialService finService = varifixLoanAccount.FixedSecuredMortgageLoan as IFinancialService;
                            double currentToDate = 0;
                            double totalForMonth = 0;

                            varifixLoanAccount.CalculateInterest(finService.Key, out currentToDate, out totalForMonth);
                            _view.InterestCurrenttoDate = currentToDate;
                            _view.InterestTotalforMonth = totalForMonth;

                            List<int> tranTypes = new List<int>();
                            tranTypes.Add((int)SAHL.Common.Globals.TransactionTypes.MonthlyInterestDebit); // c
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
