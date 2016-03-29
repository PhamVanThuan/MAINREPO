using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class SuperLoLoyaltyInfo : SAHLCommonBaseView, ISuperLoLoyaltyInfo
    {
        /// <summary>
        /// Bind Loyalty Benefit Information
        /// </summary>
        /// <param name="superLo"></param>
        public void BindLoyaltyBenefitInfo(ISuperLo superLo)
        {
            var accumulatedToDate = (from financialService in superLo.FinancialServiceAttribute.FinancialService.FinancialServices
                                     where financialService.FinancialServiceType.Key == (int)FinancialServiceTypes.LoyaltyBenefit
                                     select financialService.Balance.Amount).FirstOrDefault();
            AccumulatedToDate.Text = Convert.ToDouble(accumulatedToDate).ToString(SAHL.Common.Constants.CurrencyFormat);
            NextPaymentDate.Text = Convert.ToDateTime(superLo.NextPaymentDate).ToString(SAHL.Common.Constants.DateFormat);
            AccumulatedMonthToDate.Text = Convert.ToDouble(superLo.MTDLoyaltyBenefit).ToString(SAHL.Common.Constants.CurrencyFormat);

            double RunningTotal = 0;

            RunningTotal = Convert.ToDouble(superLo.PPThresholdYr1) + RunningTotal;
            Year1Annual.Text = Convert.ToDouble(superLo.PPThresholdYr1).ToString(SAHL.Common.Constants.CurrencyFormat);
            Year1Cumulative.Text = RunningTotal.ToString(SAHL.Common.Constants.CurrencyFormat);

            RunningTotal = Convert.ToDouble(superLo.PPThresholdYr2) + RunningTotal;
            Year2Annual.Text = Convert.ToDouble(superLo.PPThresholdYr2).ToString(SAHL.Common.Constants.CurrencyFormat);
            Year2Cumulative.Text = RunningTotal.ToString(SAHL.Common.Constants.CurrencyFormat);

            RunningTotal = Convert.ToDouble(superLo.PPThresholdYr3) + RunningTotal;
            Year3Annual.Text = Convert.ToDouble(superLo.PPThresholdYr3).ToString(SAHL.Common.Constants.CurrencyFormat);
            Year3Cumulative.Text = RunningTotal.ToString(SAHL.Common.Constants.CurrencyFormat);

            RunningTotal = Convert.ToDouble(superLo.PPThresholdYr4) + RunningTotal;
            Year4Annual.Text = Convert.ToDouble(superLo.PPThresholdYr4).ToString(SAHL.Common.Constants.CurrencyFormat);
            Year4Cumulative.Text = RunningTotal.ToString(SAHL.Common.Constants.CurrencyFormat);

            RunningTotal = Convert.ToDouble(superLo.PPThresholdYr5) + RunningTotal;
            Year5Annual.Text = Convert.ToDouble(superLo.PPThresholdYr5).ToString(SAHL.Common.Constants.CurrencyFormat);
            Year5Cumulative.Text = RunningTotal.ToString(SAHL.Common.Constants.CurrencyFormat);

            double PrePayment = Convert.ToDouble(superLo.PPAllowed);
            lblPrePaymentY1.Text = PrePayment.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblPrePaymentY2.Text = PrePayment.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblPrePaymentY3.Text = PrePayment.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblPrePaymentY4.Text = PrePayment.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblPrePaymentY5.Text = PrePayment.ToString(SAHL.Common.Constants.CurrencyFormat);

            double overpaymentamount = Convert.ToDouble(superLo.OverPaymentAmount);
            lblOverPaymentY1.Text = overpaymentamount.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblOverPaymentY2.Text = overpaymentamount.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblOverPaymentY3.Text = overpaymentamount.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblOverPaymentY4.Text = overpaymentamount.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblOverPaymentY5.Text = overpaymentamount.ToString(SAHL.Common.Constants.CurrencyFormat);
        }

        /// <summary>
        /// Bind Loyalty Benefit Payment Grid
        /// </summary>
        /// <param name="dtLoanTransactions"></param>
        public void BindLoyaltyBenefitPaymentGrid(DataTable dtLoanTransactions)
        {
            LoyaltyPaymentGrid.Columns.Clear();
            LoyaltyPaymentGrid.AddGridBoundColumn("LoanTransactionNumber", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            LoyaltyPaymentGrid.AddGridBoundColumn("LoanNumber", "Number", Unit.Percentage(5), HorizontalAlign.Left, true);
            LoyaltyPaymentGrid.AddGridBoundColumn("Service", "Service", Unit.Percentage(12), HorizontalAlign.Left, true);
            LoyaltyPaymentGrid.AddGridBoundColumn("TransactionTypeLoanDescription", "Transaction Type", Unit.Percentage(15), HorizontalAlign.Left, true);
            LoyaltyPaymentGrid.AddGridBoundColumn("LoanTransactionAmount", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Amount", false, Unit.Percentage(8), HorizontalAlign.Right, true);
            LoyaltyPaymentGrid.AddGridBoundColumn("LoanTransactionNewBalance", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Balance", false, Unit.Percentage(8), HorizontalAlign.Right, true);
            LoyaltyPaymentGrid.AddGridBoundColumn("LoanAccountCurrentBalance", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Account Balance", false, Unit.Percentage(8), HorizontalAlign.Right, true);
            LoyaltyPaymentGrid.AddGridBoundColumn("LoanTransactionRate", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Rate", false, Unit.Percentage(5), HorizontalAlign.Center, true);
            LoyaltyPaymentGrid.AddGridBoundColumn("LoanTransactionInsertDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Insert Date", false, Unit.Percentage(8), HorizontalAlign.Left, true);
            LoyaltyPaymentGrid.AddGridBoundColumn("LoanTransactionEffectiveDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Effective Date", false, Unit.Percentage(8), HorizontalAlign.Left, true);
            LoyaltyPaymentGrid.AddGridBoundColumn("LoanTransactionUserID", "Changed By", Unit.Percentage(10), HorizontalAlign.Left, true);
            LoyaltyPaymentGrid.AddGridBoundColumn("LoanTransactionReference", "Reference", Unit.Percentage(18), HorizontalAlign.Left, true);
            LoyaltyPaymentGrid.DataSource = dtLoanTransactions;
            LoyaltyPaymentGrid.DataBind();
        }

        /// <summary>
        /// RowDataBound Event for Loyalty Benefit Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoyaltyPaymentGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        /// <summary>
        /// Cancel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            if (CancelButtonClicked != null)
                CancelButtonClicked(sender, new KeyChangedEventArgs(e));
        }

        /// <summary>
        /// Super Lo Opt Out button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SuperLoOptOutButton_Click(object sender, EventArgs e)
        {
            if (SuperLoOptOutButtonClicked != null)
                SuperLoOptOutButtonClicked(sender, new KeyChangedEventArgs(e));
        }

        /// <summary>
        /// Event handler for Cancel Button Clicked
        /// </summary>
        public event KeyChangedEventHandler CancelButtonClicked;

        /// <summary>
        /// Event handler for Update Button being Clicked
        /// </summary>
        public event KeyChangedEventHandler UpdateButtonClicked;

        /// <summary>
        /// Event handler for Super Lo Opt Out Button Clicked
        /// </summary>
        public event KeyChangedEventHandler SuperLoOptOutButtonClicked;

        protected void btnUpdateThreshold_Click(object sender, EventArgs e)
        {
            if (UpdateButtonClicked != null)
                UpdateButtonClicked(sender, new KeyChangedEventArgs(e));
        }

        /// <summary>
        /// Set the value of the Exclude from Opt Out Check Box
        /// </summary>
        public bool ExcludeFromOptOut
        {
            get { return chbExclude.Checked; }
            set { chbExclude.Checked = value; }
        }

        /// <summary>
        /// Set the value of the Exclusion Date
        /// </summary>
        public DateTime? ExclusionDate
        {
            get
            {
                return Convert.ToDateTime(dtExclusionDate.Date);
            }
            set { dtExclusionDate.Date = value; }
        }

        /// <summary>
        /// Set the value of the Exclusion Reason
        /// </summary>
        public string ExclusionReason
        {
            get { return txtExcludeReason.Text; }
            set { txtExcludeReason.Text = value; }
        }

        /// <summary>
        /// This Sets the threshold management edit state
        /// </summary>
        public bool SetThresholdManagementEditable
        {
            set
            {// Threshold management block
                chbExclude.Enabled = value;
                dtExclusionDate.Enabled = value;
                txtExcludeReason.Enabled = value;
                btnUpdateThreshold.Enabled = value;
            }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Common.Interfaces.ISuperLoLoyaltyInfo.SuperLoOptOutButtonVisible">ISuperLoLoyaltyInfo.SuperLoOptOutButtonVisible</see>.
        /// </summary>
        public bool SuperLoOptOutButtonVisible
        {
            set { SuperLoOptOut.Visible = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Common.Interfaces.ISuperLoLoyaltyInfo.UpdateThresholdButtonVisible">ISuperLoLoyaltyInfo.UpdateThresholdButtonVisible</see>.
        /// </summary>
        public bool UpdateThresholdButtonVisible
        {
            set { btnUpdateThreshold.Visible = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Common.Interfaces.ISuperLoLoyaltyInfo.CancelButtonVisible">ISuperLoLoyaltyInfo.CancelButtonVisible</see>.
        /// </summary>
        public bool CancelButtonVisible
        {
            set { CancelButton.Visible = value; }
        }

        public void AddOptOutConfirmation()
        {
            SuperLoOptOut.Attributes.Add("onclick", @"return confirm('WARNING: By performing this action, you confirm that:\n1. ALL benefits of the Super Lo product will be forfeited.\n2. All relevant documentation for the Opt Out has been received.\nAre you sure you would like to continue the Super Lo Opt Out?');");
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Common.Interfaces.ISuperLoLoyaltyInfo.ThresholdManagementPanelVisible">ISuperLoLoyaltyInfo.ThresholdManagementPanelVisible</see>.
        /// </summary>
        public bool ThresholdManagementPanelVisible
        {
            set { pnlThresholdManagement.Visible = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Common.Interfaces.ISuperLoLoyaltyInfo.LoyaltyBenefitPanelVisible">ISuperLoLoyaltyInfo.LoyaltyBenefitPanelVisible</see>.
        /// </summary>
        public bool LoyaltyBenefitPanelVisible
        {
            set { LoyaltyBenefitPanel.Visible = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Common.Interfaces.ISuperLoLoyaltyInfo.PrepaymentThresholdsPanelVisible">ISuperLoLoyaltyInfo.PrepaymentThresholdsPanelVisible</see>.
        /// </summary>
        public bool PrepaymentThresholdsPanelVisible
        {
            set { PrepaymentThresholds.Visible = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Common.Interfaces.ISuperLoLoyaltyInfo.LoyaltyPaymentGridVisible">ISuperLoLoyaltyInfo.LoyaltyPaymentGridVisible</see>.
        /// </summary>
        public bool LoyaltyPaymentGridVisible
        {
            set { LoyaltyPaymentGrid.Visible = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool CreateSpaceTable
        {
            set { tblCreateSpace.Visible = value; }
        }
    }
}