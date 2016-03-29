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
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Globals;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using System.Linq;


namespace SAHL.Web.Views.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class LoanFinancialServiceSummary : SAHLCommonBaseView, ILoanFinancialServiceSummary
    {
        #region Variables
        double _interestCurrenttoDate;
        double _interestTotalforMonth;
        double _interestPreviousMonth;
        double _amortisationInstalment;
        #endregion

        private enum FinancialAdjustmentGridColumnPositions
        {
            AccountKey = 0,
            FinancialAdjustmentSource = 1,
            FinancialAdjustmentType = 2,
            FromDate = 3,
            Term = 4,
            Value = 5,
            Status = 6

        }

        #region Properties
        /// <summary>
        /// Sets the Interest Current to Date Value
        /// </summary>
        public double InterestCurrenttoDate
        {
            set
            {
                _interestCurrenttoDate = value;
            }
        }

        /// <summary>
        /// Sets the Interest Current to Date Value
        /// </summary>
        public double InterestTotalforMonth
        {
            set
            {
                _interestTotalforMonth = value;
            }
        }

        /// <summary>
        /// Sets the Interest Previous Month Value
        /// </summary>
        public double InterestPreviousMonth
        {
            set
            {
                _interestPreviousMonth = value;
            }
        }

        /// <summary>
        /// Sets the Amortisation Amount Value and makes the control visible
        /// </summary>
        public double AmortisationInstalment
        {
            set
            {
                _amortisationInstalment = value;
                
            }
        }

        /// <summary>
        /// Visibility of AmortisingInstallment fields
        /// </summary>
        public bool AmortisingInstallmentVisible
        {
            set {
                AmortisingInstallment.Visible = value;
                lblAmortisingInstallment.Visible = value;
            }
        }


        
        /// <summary>
        /// Visibility of AmortisingInstallment fields
        /// </summary>
        public bool LoyaltyButtonVisible
        {
            set {
                LoyaltyButton.Visible = value;
            }
        }
        #endregion

        #region ILoanFinancialServiceSummary Members

        /// <summary>
        /// Binds Mortgage Loan Details to Financial Service Summary View
        /// </summary>
        /// <param name="MortgageLoan"></param>
        public void BindFinancialServiceSummaryData(IMortgageLoan MortgageLoan)
        {
            if (MortgageLoan != null)
            {
                Service.Text = MortgageLoan.FinancialServiceType.Description;
                Purpose.Text = MortgageLoan.MortgageLoanPurpose.Description;
				OpenDate.Text = MortgageLoan.OpenDate.HasValue ? MortgageLoan.OpenDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
                if (MortgageLoan.CloseDate.HasValue)
                    CloseDate.Text = MortgageLoan.CloseDate.Value.ToString(SAHL.Common.Constants.DateFormat);
                Status.Text = MortgageLoan.AccountStatus.Description;

				var originationApplication = MortgageLoan.Account.Applications.FirstOrDefault(x => x.ApplicationType.Key != (int)OfferTypes.FurtherLoan &&
										   x.ApplicationType.Key != (int)OfferTypes.FurtherAdvance &&
										   x.ApplicationType.Key != (int)OfferTypes.ReAdvance);
				if (originationApplication != null)
				{
					ISupportsVariableLoanApplicationInformation supportsVariableLoanInformation = originationApplication.CurrentProduct as ISupportsVariableLoanApplicationInformation;
					if (supportsVariableLoanInformation != null)
					{
						var applicationInformationVariableLoan = supportsVariableLoanInformation.VariableLoanInformation;
						InitialTerm.Text = applicationInformationVariableLoan.Term + " Months";
					}
				}

				lblTotalTerm.Text = MortgageLoan.InitialInstallments + " Months";
                RemainingTerm.Text = MortgageLoan.RemainingInstallments + " Months";

                InitialBalance.Text = MortgageLoan.Balance.LoanBalance.InitialBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
                Installment.Text = MortgageLoan.Payment.ToString(SAHL.Common.Constants.CurrencyFormat);
                CurrentBalance.Text = MortgageLoan.CurrentBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
                ArrearBalance.Text = MortgageLoan.ArrearBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
                MarketRateDescription.Text = MortgageLoan.RateConfiguration.MarketRate.Description;
                
                IRateConfiguration RateConfiguration = MortgageLoan.RateConfiguration;
                if (RateConfiguration != null)
                {
                    double MarginValue = RateConfiguration.Margin.Value;
                    LinkRate.Text = ((double)((MarginValue + MortgageLoan.RateAdjustment) )).ToString(SAHL.Common.Constants.RateFormat);
                }
                else
                {
                    throw new Exception("Rate Configuration Not Found");
                }

                MarketRate.Text = ((double)(MortgageLoan.ActiveMarketRate )).ToString(SAHL.Common.Constants.RateFormat) ;
                EffectiveRate.Text = ((double)(MortgageLoan.InterestRate )).ToString(SAHL.Common.Constants.RateFormat) ;

                if (MortgageLoan.NextResetDate.HasValue)
                    NextResetDate.Text = MortgageLoan.NextResetDate.Value.ToString(SAHL.Common.Constants.DateFormat);
				//Removed as this seems to be an RCS value and has been dropped in the Revamp
				//if (MortgageLoan.PreApproved.HasValue )
				//    lblPreApproved.Text = MortgageLoan.PreApproved.Value.ToString(SAHL.Common.Constants.CurrencyFormat);

                AmortisingInstallment.Text = _amortisationInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
                IntCurrentToDate.Text = _interestCurrenttoDate.ToString(SAHL.Common.Constants.CurrencyFormat);
                IntCurrentToMonth.Text = _interestTotalforMonth.ToString(SAHL.Common.Constants.CurrencyFormat);
                IntPreviousMonth.Text = _interestPreviousMonth.ToString(SAHL.Common.Constants.CurrencyFormat);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstFinancialAdjustments"></param>
		public void BindFinancialAdjustmentGrid(IList<IFinancialAdjustment> lstFinancialAdjustments)
        {
            FinancialAdjustmentGrid.AddGridBoundColumn("", "Account", Unit.Percentage(8), HorizontalAlign.Left, true);
            FinancialAdjustmentGrid.AddGridBoundColumn("", "Financial Adjustment", Unit.Percentage(20), HorizontalAlign.Left, true);
            FinancialAdjustmentGrid.AddGridBoundColumn("", "Financial Adjustment Type", Unit.Percentage(20), HorizontalAlign.Left, true);
            FinancialAdjustmentGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Effective Date", false, Unit.Percentage(12), HorizontalAlign.Center, true);
            FinancialAdjustmentGrid.AddGridBoundColumn("", "Term", Unit.Percentage(5), HorizontalAlign.Left, true);
            FinancialAdjustmentGrid.AddGridBoundColumn("", "Value", Unit.Percentage(10), HorizontalAlign.Left, true);
            FinancialAdjustmentGrid.AddGridBoundColumn("", "Status", Unit.Percentage(11), HorizontalAlign.Left, true);
			FinancialAdjustmentGrid.DataSource = lstFinancialAdjustments;
            FinancialAdjustmentGrid.DataBind();
        }

		#region Protected Functions Section

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        protected void FinancialAdjustmentGrid_RowDataBound(object sender, GridViewRowEventArgs e)
		{
            if (e.Row.DataItem != null)
            {
                // Get the FinancialAdjustment Row
                IFinancialAdjustment financialAdjustment = e.Row.DataItem as IFinancialAdjustment;

                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.AccountKey].Text = financialAdjustment.FinancialService.Account.Key.ToString();
                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.FinancialAdjustmentSource].Text = financialAdjustment.FinancialAdjustmentSource.Description;
                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.FinancialAdjustmentType].Text = financialAdjustment.FinancialAdjustmentType.Description;
                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.FromDate].Text = financialAdjustment.FromDate.HasValue ? financialAdjustment.FromDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "";
                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.Term].Text = financialAdjustment.Term.ToString();
                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.Value].Text = financialAdjustment.ToString();
                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.Status].Text = financialAdjustment.FinancialAdjustmentStatus.Description;

                switch (financialAdjustment.FinancialAdjustmentStatus.Key)
                {
                    case (int)FinancialAdjustmentStatuses.Active:
                        e.Row.Attributes.Add("style", "color: #FFFFFF; background-color:green;");
                        break;
                    case (int)FinancialAdjustmentStatuses.Inactive:
                        e.Row.Attributes.Add("style", "color: #FFFFFF; background-color:#e66426;");
                        break;
                    case (int)FinancialAdjustmentStatuses.Canceled:
                        e.Row.Attributes.Add("style", "color: #FFFFFF; background-color:red;");
                        break;
                    default:
                        break;
                }
            }
		}

		#endregion


        /// <summary>
        /// Loyalty Button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoyaltyButton_Click(object sender, EventArgs e)
        {
            if (LoyaltyButtonClicked != null)
                LoyaltyButtonClicked(sender, new KeyChangedEventArgs(e));
        }

        /// <summary>
        /// Event handler for Loyalty Button Clicked
        /// </summary>
        public event EventHandler LoyaltyButtonClicked;
        #endregion

    }
}
