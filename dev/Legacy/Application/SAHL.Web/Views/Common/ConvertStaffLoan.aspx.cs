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
	public partial class ConvertStaffLoan : SAHLCommonBaseView, IConvertStaffLoan
	{
		public bool ConvertButtonEnabled
		{
			get
			{
				return btnConvert.Enabled;
			}
			set
			{
				btnConvert.Enabled = value;
			}
		}
		public bool UnConvertButtonEnabled
		{
			get
			{
				return btnUnConvert.Enabled;
			}
			set
			{
				btnUnConvert.Enabled = value;
			}
		}

		double _totalCurrentBalance;
		double _totalArrearBalance;
		double _totalInstallment;

		public void BindAccountDetails(IAccount account, IMortgageLoan vML, IMortgageLoan fML)
		{
			//Variable Loan Totals
			_totalCurrentBalance = Convert.ToDouble(vML.CurrentBalance + (vML.AccruedInterestMTD.HasValue ? vML.AccruedInterestMTD.Value : 0));
			_totalArrearBalance = vML.ArrearBalance;
			_totalInstallment = vML.Payment;

			if (fML != null)
			{
				// Fixed Loan Totals
				_totalCurrentBalance += Convert.ToDouble(fML.CurrentBalance + (fML.AccruedInterestMTD.HasValue ? fML.AccruedInterestMTD.Value : 0));
				_totalArrearBalance += Convert.ToDouble(fML.ArrearBalance + (fML.AccruedInterestMTD.HasValue ? fML.AccruedInterestMTD : 0));
				_totalInstallment += fML.Payment;
				// Fixed Loan Rates
				lblfMLEffectiveRate.Text = fML.InterestRate.ToString(SAHL.Common.Constants.RateFormat);
				lblfMLLinkRate.Text = Convert.ToDouble(fML.InterestRate - fML.ActiveMarketRate).ToString(SAHL.Common.Constants.RateFormat);
				lblfMLMarketRate.Text = fML.ActiveMarketRate.ToString(SAHL.Common.Constants.RateFormat);
			}
			else
			{
				pnlFixedML.Visible = false;
			}
			//Display Totals
			lblAccountStatus.Text = account.AccountStatus.Description;
			lblCurrentBalance.Text = _totalCurrentBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
			lblArrearBalance.Text = _totalArrearBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
			lblRemainingInstallments.Text = vML.RemainingInstallments.ToString();
			lblPayment.Text = _totalInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);

			//Variable Loan Rates
			lblvMLLinkRate.Text = Convert.ToDouble(vML.InterestRate - vML.ActiveMarketRate).ToString(SAHL.Common.Constants.RateFormat);
			lblvMLEffectiveRate.Text = vML.InterestRate.ToString(SAHL.Common.Constants.RateFormat);
			lblvMLMarketRate.Text = vML.ActiveMarketRate.ToString(SAHL.Common.Constants.RateFormat);
		}

		protected void btnUnConvert_Click(object sender, EventArgs e)
		{
			if (onUnConvertButtonClicked != null)
			{
				onUnConvertButtonClicked(sender, e);
			}
		}

		protected void btnConvert_Click(object sender, EventArgs e)
		{
			if (onConvertButtonClicked != null)
			{
				onConvertButtonClicked(sender, e);
			}
		}

		#region Events Intialization

		public event EventHandler onConvertButtonClicked;

		public event EventHandler onUnConvertButtonClicked;

		#endregion
	}
}
