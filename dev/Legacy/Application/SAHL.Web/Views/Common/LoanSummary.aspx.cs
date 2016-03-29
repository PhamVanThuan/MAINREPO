using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common
{
    public partial class LoanSummary : SAHLCommonBaseView, ILoanSummary
    {
        #region Variables

        double _interestCurrenttoDateVariable;
        double _interestTotalforMonthVariable;
        double _interestPreviousMonthVariable;
        double _interestCurrenttoDateFixed;
        double _interestTotalforMonthFixed;
        double _interestPreviousMonthFixed;

        double _latestProperyValuation;
        double _loanAgreementAmount;
        double _committedLoanValue;
        double _totalBondAmount;
        double _householdIncome;
        double _LTV;
        double _PTI;

        string _spvDescription;
        DateTime _valuationDate;
        int _debitOrderDay;
        DateTime _maturityDate;

        private double _gridtotalInstallment;
        private double _gridtotalArrearBalance;
        private double _gridtotalCurrentBalance;

        private bool _nonPerformingLoan;
        private bool _titleDeedOnFile;
        private bool _naedoCompliant;

        #endregion

        #region Protected Functions Section

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ShortTermLoansGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                IMortgageLoan ro = e.Row.DataItem as IMortgageLoan;
                e.Row.Cells[0].Text = ro.MortgageLoanPurpose.Description;
                e.Row.Cells[1].Text = ro.AccountStatus.Description;
                e.Row.Cells[4].Text = e.Row.Cells[4].Text + " Months";
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[2].Text = _gridtotalCurrentBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[3].Text = _gridtotalArrearBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[6].Text = _gridtotalInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoansGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                IMortgageLoan ro = e.Row.DataItem as IMortgageLoan;
                e.Row.Cells[0].Text = ro.FinancialServiceType.Description;
                e.Row.Cells[1].Text = ro.AccountStatus.Description;
                e.Row.Cells[4].Text = e.Row.Cells[4].Text + " Months";
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[2].Text = _gridtotalCurrentBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[3].Text = _gridtotalArrearBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[6].Text = _gridtotalInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);
            }


        }

        #endregion

        #region Properties
        /// <summary>
        /// Sets the Interest Current to Date Value for the Variable loan
        /// </summary>
        public double InterestCurrenttoDateVariable
        {
            set
            {
                _interestCurrenttoDateVariable = value;
            }
        }

        /// <summary>
        /// Sets the Interest Current to Date Value for the Variable loan
        /// </summary>
        public double InterestTotalforMonthVariable
        {
            set
            {
                _interestTotalforMonthVariable = value;
            }
        }

        /// <summary>
        /// Sets the Interest Previous Month Value for the Variable loan
        /// </summary>
        public double InterestPreviousMonthVariable
        {
            set
            {
                _interestPreviousMonthVariable = value;
            }
        }

        /// <summary>
        /// Sets the Interest Current to Date Value for the Fixed loan
        /// </summary>
        public double InterestCurrenttoDateFixed
        {
            set
            {
                _interestCurrenttoDateFixed = value;
            }
        }

        /// <summary>
        /// Sets the Interest Current to Date Value for the Fixed loan
        /// </summary>
        public double InterestTotalforMonthFixed
        {
            set
            {
                _interestTotalforMonthFixed = value;
            }
        }

        /// <summary>
        /// Sets the Interest Previous Month Value for the Fixed loan
        /// </summary>
        public double InterestPreviousMonthFixed
        {
            set
            {
                _interestPreviousMonthFixed = value;
            }
        }

        /// <summary>
        /// Visibility of Fixed Loan Fields fields
        /// </summary>
        public bool FixedLoanControlsVisible
        {
            set
            {
                FixedCell1.Visible = value;
                FixedCell2.Visible = value;
                FixedCell3.Visible = value;
                FixedCell4.Visible = value;
            }
        }

        /// <summary>
        /// Visibility of Maturity Date field
        /// </summary>
        public bool MaturityDateVisible
        {
            set
            {
                lblMaturityDate.Visible = value;
            }
        }

        /// <summary>
        /// Text of Maturity Date label
        /// </summary>
        public string MaturityDateTitleText
        {
            set
            {
                lblMaturityDateTitle.Text = value;
            }
        }


        public bool ManualLifePolicyPaymentVisible
        {
            set
            {
                rowManualLifePolicyPayment.Visible = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool TitleDeedOnFile
        {
            set { _titleDeedOnFile = value; }
        }


        #region Total Details
        /// <summary>
        /// 
        /// </summary>
        public double LatestProperyValuationAmount
        {
            set
            {
                _latestProperyValuation = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double LoanAgreementAmount
        {
            set
            {
                _loanAgreementAmount = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double CommittedLoanValue
        {
            set
            {
                _committedLoanValue = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double TotalBondAmount
        {
            set
            {
                _totalBondAmount = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double HouseholdIncome
        {
            set
            {
                _householdIncome = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double PTI
        {
            set
            {
                _PTI = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double LTV
        {
            set
            {
                _LTV = value;
            }
        }

        public bool NonPerformingLoan
        {
            get { return _nonPerformingLoan; }
            set { _nonPerformingLoan = value; }
        }

        #endregion

        #region MortgageLoanDetails
        /// <summary>
        /// 
        /// </summary>
        public string SpvDescription
        {
            set
            {
                _spvDescription = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ValuationDate
        {
            set
            {
                _valuationDate = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime MaturityDate
        {
            set
            {
                _maturityDate = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int DebitOrderDay
        {
            set
            {
                _debitOrderDay = value;
            }
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        public bool NaedoCompliant
        {
            set { _naedoCompliant = value; }
        }

        public bool IsGEPFFunded { set; get; }

        #endregion

        #region ILoanSummary Members

        /// <summary>
        /// Binds Account Details to Loan Summary View
        /// </summary>
        /// <param name="account"></param>
        public void BindAccountLoanSummaryData(IAccount account)
        {
            if (account != null)
            {
                lblAccountNumber.Text = account.Key.ToString();
                lblAccountStatus.Text = account.AccountStatus.Description;
                if (account.OpenDate.HasValue)
                    lblOpenDate.Text = account.OpenDate.Value.ToString(SAHL.Common.Constants.DateFormat);
                lblProduct.Text = account.Product.Description;

                if (account.CloseDate.HasValue)
                    lblCloseDate.Text = account.CloseDate.Value.ToString(SAHL.Common.Constants.DateFormat);
            }
        }

        /// <summary>
        /// Binds the total data to the Loan Summary View
        /// </summary>
        public void BindTotalData()
        {
            lblTotalBondAmount.Text = _totalBondAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblLatestProperyValuation.Text = _latestProperyValuation.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblLoanAgreementAmount.Text = _loanAgreementAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblCLV.Text = _committedLoanValue.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblHouseholdIncome.Text = _householdIncome.ToString(SAHL.Common.Constants.CurrencyFormat);

            lblCurrentToMonthVariable.Text = _interestTotalforMonthVariable.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblCurrentToDateVariable.Text = _interestCurrenttoDateVariable.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblPreviousMonthVariable.Text = _interestPreviousMonthVariable.ToString(SAHL.Common.Constants.CurrencyFormat);

            lblCurrentToMonthFixed.Text = _interestTotalforMonthFixed.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblCurrentToDateFixed.Text = _interestCurrenttoDateFixed.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblPreviousMonthFixed.Text = _interestPreviousMonthFixed.ToString(SAHL.Common.Constants.CurrencyFormat);

            lblLTV.Text = _LTV.ToString(SAHL.Common.Constants.RateFormat);
            lblPTI.Text = _PTI.ToString(SAHL.Common.Constants.RateFormat);
        }

        /// <summary>
        /// Binds the MortgageLoan data to the View
        /// </summary>
        public void BindMorgageLoanData()
        {
            lblSPVDescription.Text = _spvDescription;
            if (_valuationDate != new DateTime(01, 01, 01))
                lblValuationDate.Text = _valuationDate.ToString(SAHL.Common.Constants.DateFormat);
            lblDebitOrderDay.Text = _debitOrderDay.ToString();
            if (_maturityDate != new DateTime(01, 01, 01))
                lblMaturityDate.Text = _maturityDate.ToString(SAHL.Common.Constants.DateFormat);

            NonPerformingCheck.Checked = _nonPerformingLoan;
            NonPerformingCheck.Enabled = false;

            lbHaveTitleDeed.Text = _titleDeedOnFile ? "Yes" : "No";

            lblNaedoCompliant.Text = _naedoCompliant ? "Yes" : "No";

            lblGEPFFunded.Text = IsGEPFFunded ? "Yes" : "No";
        }


        /// <summary>
        /// Bind the Financial Service Data to the Loans Grid
        /// </summary>
        /// <param name="lstMortgageLoans"></param>
        /// <param name="gridtotalInstallment"></param>
        /// <param name="gridtotalArrearBalance"></param>
        /// <param name="gridtotalCurrentBalance"></param>
        public void BindLoansGrid(IList<IMortgageLoan> lstMortgageLoans,
                                    double gridtotalInstallment,
                                    double gridtotalArrearBalance,
                                    double gridtotalCurrentBalance)
        {
            _gridtotalArrearBalance = gridtotalArrearBalance;
            _gridtotalCurrentBalance = gridtotalCurrentBalance;
            _gridtotalInstallment = gridtotalInstallment;

            LoansGrid.ShowFooter = true;
            LoansGrid.AddGridBoundColumn("", "Description", Unit.Percentage(13), HorizontalAlign.Left, true);
            LoansGrid.AddGridBoundColumn("", "Status", Unit.Percentage(5), HorizontalAlign.Left, true);
            LoansGrid.AddGridBoundColumn("CurrentBalance", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Current Balance", false, Unit.Percentage(14), HorizontalAlign.Right, true);
            LoansGrid.AddGridBoundColumn("ArrearBalance", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Arrear Balance", false, Unit.Percentage(14), HorizontalAlign.Right, true);
            LoansGrid.AddGridBoundColumn("RemainingInstallments", "Remaining Term", Unit.Percentage(9), HorizontalAlign.Left, true);
            LoansGrid.AddGridBoundColumn("InterestRate", SAHL.Common.Constants.RateFormat, GridFormatType.GridRate, "Effective Rate", false, Unit.Percentage(9), HorizontalAlign.Right, true);
            LoansGrid.AddGridBoundColumn("Payment", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Instalment", false, Unit.Percentage(14), HorizontalAlign.Right, true);
            LoansGrid.AddGridBoundColumn("NextResetDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Next Reset Date", false, Unit.Percentage(10), HorizontalAlign.Center, true);
            /* 
             "PreApproved" has been removed of the MortgageLoan table; we awaiting confirmation from BackEnd Team should it be
             removed it completely or use different structure ??? - #19675
             */
            //LoansGrid.AddGridBoundColumn("PreApproved", "Pre Approved Amount", Unit.Percentage(12), HorizontalAlign.Left, true);
            LoansGrid.DataSource = lstMortgageLoans;
            LoansGrid.DataBind();
        }

        /// <summary>
        /// Bind the Financial Service Data to the Short Term Loans Grid
        /// </summary>
        /// <param name="lstShortTermMortgageLoans"></param>
        /// <param name="gridtotalInstallment"></param>
        /// <param name="gridtotalArrearBalance"></param>
        /// <param name="gridtotalCurrentBalance"></param>
        public void BindShortTermLoansGrid(IList<IMortgageLoan> lstShortTermMortgageLoans,
                                    double gridtotalInstallment,
                                    double gridtotalArrearBalance,
                                    double gridtotalCurrentBalance)
        {
            _gridtotalArrearBalance = gridtotalArrearBalance;
            _gridtotalCurrentBalance = gridtotalCurrentBalance;
            _gridtotalInstallment = gridtotalInstallment;

            ShortTermLoansGrid.ShowFooter = true;
            ShortTermLoansGrid.AddGridBoundColumn("", "Description", Unit.Percentage(13), HorizontalAlign.Left, true);
            ShortTermLoansGrid.AddGridBoundColumn("", "Status", Unit.Percentage(5), HorizontalAlign.Left, true);
            ShortTermLoansGrid.AddGridBoundColumn("CurrentBalance", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Current Balance", false, Unit.Percentage(14), HorizontalAlign.Right, true);
            ShortTermLoansGrid.AddGridBoundColumn("ArrearBalance", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Arrear Balance", false, Unit.Percentage(14), HorizontalAlign.Right, true);
            ShortTermLoansGrid.AddGridBoundColumn("RemainingInstallments", "Remaining Term", Unit.Percentage(9), HorizontalAlign.Left, true);
            ShortTermLoansGrid.AddGridBoundColumn("InterestRate", SAHL.Common.Constants.RateFormat, GridFormatType.GridRate, "Effective Rate", false, Unit.Percentage(9), HorizontalAlign.Right, true);
            ShortTermLoansGrid.AddGridBoundColumn("Payment", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Instalment", false, Unit.Percentage(14), HorizontalAlign.Right, true);
            ShortTermLoansGrid.AddGridBoundColumn("NextResetDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Next Reset Date", false, Unit.Percentage(10), HorizontalAlign.Center, true);
            //ShortTermLoansGrid.AddGridBoundColumn("PreApproved", "Pre Approved Amount", Unit.Percentage(12), HorizontalAlign.Left, true);

            ShortTermLoansGrid.DataSource = lstShortTermMortgageLoans;
            ShortTermLoansGrid.DataBind();
        }

        #endregion
    }
}
