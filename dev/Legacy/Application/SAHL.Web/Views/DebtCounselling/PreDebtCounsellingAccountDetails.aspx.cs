using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.DebtCounselling
{
	public partial class PreDebtCounsellingAccountDetails : SAHLCommonBaseView, IPreDebtCounsellingAccountDetails
	{
		/// <summary>
		/// Bind the Account Snap Shot
		/// </summary>
		/// <param name="snapShotAccount"></param>
        public void BindSnapShotAccount(SAHL.Common.BusinessModel.Interfaces.ISnapShotAccount snapShotAccount, double preDCInstalment, double linkRate, double marketRate, double interestRate, int term,double lifeInstallment,double hocInstallment)
		{
			if (snapShotAccount == null)
			{
				pnlEmptyDebtCounsellingSnapShot.Visible = true;
				return;
			}

            //snapShotAccount.Account

			pnlDebtCounsellingSnapShot.Visible = true;

			var variableFinancialService = snapShotAccount.SnapShotFinancialServices.Where(x => x.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan).FirstOrDefault();
            var fixedFinancialService = snapShotAccount.SnapShotFinancialServices.Where(x => x.FinancialServiceType.Key == (int)FinancialServiceTypes.FixedLoan).FirstOrDefault();

			if (fixedFinancialService == null)
			{
				divFixed.Visible = false;
			}

			/* Variable */
			lblVariableInstallment.Text = variableFinancialService.Installment.ToString(SAHL.Common.Constants.CurrencyFormat);
			lblVariableLinkRate.Text = variableFinancialService.Margin.Value.ToString(SAHL.Common.Constants.RateFormat);
			lblVariableMarketRate.Text = variableFinancialService.ActiveMarketRate.ToString(SAHL.Common.Constants.RateFormat);
			lblVariableInterestRate.Text = (variableFinancialService.Margin.Value + variableFinancialService.ActiveMarketRate).ToString(SAHL.Common.Constants.RateFormat);

			lblTerm.Text = snapShotAccount.RemainingInstallments.ToString();
			lblHOCPremium.Text = snapShotAccount.HOCPremium.ToString(SAHL.Common.Constants.CurrencyFormat);
			lblLifePremium.Text = snapShotAccount.LifePremium.ToString(SAHL.Common.Constants.CurrencyFormat);
			lblMonthlyServiceFee.Text = snapShotAccount.MonthlyServiceFee.ToString(SAHL.Common.Constants.CurrencyFormat);

			lblProduct.Text = snapShotAccount.Product.Description;

			if (fixedFinancialService != null)
			{
				/* Fixed */
				lblFixedInstallment.Text = fixedFinancialService.Installment.ToString(SAHL.Common.Constants.CurrencyFormat);
				lblFixedLinkRate.Text = fixedFinancialService.Margin.Value.ToString(SAHL.Common.Constants.RateFormat);
				lblFixedMarketRate.Text = fixedFinancialService.ActiveMarketRate.ToString(SAHL.Common.Constants.RateFormat);
				lblFixedInterestRate.Text = (fixedFinancialService.Margin.Value + fixedFinancialService.ActiveMarketRate).ToString(SAHL.Common.Constants.RateFormat);
			}

			var financialAdjustmentsToDisplay = new List<FinancialAdjustmentDisplay>();
			var activeFinancialAdjustments = variableFinancialService.SnapShotFinancialAdjustments;//.Where(x=>x.GeneralStatusKey == (int)GeneralStatuses.Active);

			//Get the fixed financial service rate overrides
			if (fixedFinancialService != null)
			{
				activeFinancialAdjustments.Concat(fixedFinancialService.SnapShotFinancialAdjustments);//.Where(x => x.GeneralStatusKey == (int)GeneralStatuses.Active));
			}

			foreach (var financialAdjustment in activeFinancialAdjustments)
			{
                var financialAdjustmentToDisplay = new FinancialAdjustmentDisplay
                {
                    EndDate = financialAdjustment.EndDate.HasValue ? financialAdjustment.EndDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "-",
                    FromDate = financialAdjustment.FromDate.HasValue ? financialAdjustment.FromDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "-",
                    FinancialAdjustmentType = string.Format("{0} - {1}",
                                                            financialAdjustment.FinancialAdjustmentSource.Description,
                                                            financialAdjustment.FinancialAdjustmentType.Description)
                };
				switch (financialAdjustment.FinancialAdjustmentType.Key)
				{
					case (int)FinancialAdjustmentTypes.DifferentialProvision:
						financialAdjustmentToDisplay.Value = financialAdjustment.DPADifferentialAdjustment.ToString(SAHL.Common.Constants.RateFormat);
						break;
					case (int)FinancialAdjustmentTypes.FixedRateAdjustment:
						financialAdjustmentToDisplay.Value = financialAdjustment.FRARate.ToString(SAHL.Common.Constants.RateFormat);
						break;
					case (int)FinancialAdjustmentTypes.InterestRateAdjustment:
						financialAdjustmentToDisplay.Value = financialAdjustment.IRAAdjustment.ToString(SAHL.Common.Constants.RateFormat);
						break;
					case (int)FinancialAdjustmentTypes.PaymentAdjustment:
						financialAdjustmentToDisplay.Value = financialAdjustment.DPABalanceType.Key.ToString(SAHL.Common.Constants.CurrencyFormat);
						break;
					case (int)FinancialAdjustmentTypes.ReversalProvision:
						financialAdjustmentToDisplay.Value = financialAdjustment.RPAReversalPercentage.ToString(SAHL.Common.Constants.CurrencyFormat);
						break;
				}
				financialAdjustmentsToDisplay.Add(financialAdjustmentToDisplay);
			}

            gridFinancialAdjustments.AddGridColumn("FromDate", "FromDate", 0, GridFormatType.GridString, null, HorizontalAlign.Center, true, true);
            gridFinancialAdjustments.AddGridColumn("EndDate", "End Date", 0, GridFormatType.GridString, null, HorizontalAlign.Center, true, true); 
            gridFinancialAdjustments.AddGridColumn("Value", "Value", 0, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            gridFinancialAdjustments.AddGridColumn("FinancialAdjustmentType", "Financial Adjustment Type", 0, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            gridFinancialAdjustments.DataSource = financialAdjustmentsToDisplay;
            gridFinancialAdjustments.DataBind();

            lblVariableInstallmentOptOutToday.Text = preDCInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblVariableLinkRateOptOutToday.Text = linkRate.ToString(SAHL.Common.Constants.RateFormat);
            lblVariableMarketRateOptOutToday.Text = marketRate.ToString(SAHL.Common.Constants.RateFormat);
            lblVariableInterestRateOptOutToday.Text = interestRate.ToString(SAHL.Common.Constants.RateFormat);

            IMortgageLoanAccount mortgageLoanAccount = snapShotAccount.Account as IMortgageLoanAccount;
            IMortgageLoan mortLoan = mortgageLoanAccount.SecuredMortgageLoan as IMortgageLoan;

            lblTermOptOutToday.Text = term.ToString();
            lblCurrentBalance.Text = mortLoan.CurrentBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblHOCPremiumOptOutToday.Text = hocInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblLifePremiumOptOutToday.Text = lifeInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblMonthlyServiceFeeOptOutToday.Text = snapShotAccount.Account.InstallmentSummary.MonthlyServiceFee.ToString(SAHL.Common.Constants.CurrencyFormat);
		}

        public string Set_DebtCounsellingCancelledHeading_InnerText
        {
            set 
            { 
                DebtCounsellingCancelledHeading.InnerText = value; 
            }
        }

        public bool Set_DebtCounsellingInfo_Visibility
        {
            set
            {
                DebtCounsellingInfo.Visible = value;
            }
        }

        protected class FinancialAdjustmentDisplay
        {
            public string FinancialAdjustmentType { get; set; }
            public string FromDate { get; set; }
            public string EndDate { get; set; }
            public string Value { get; set; }
        }
	}
}