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
	public partial class NonPerformingLoan : SAHLCommonBaseView, INonPerformingLoan
	{
		protected void btnProceed_Click(object sender, EventArgs e)
		{
			if (onProceedButtonClicked != null)
			{
				onProceedButtonClicked(sender, e);
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			if (onCancelButtonClicked != null)
			{
				onCancelButtonClicked(sender, e);
			}
		}

		public bool CheckBoxValue
		{
			set
			{
				NonPerformingCheck.Checked = value;
			}
			get
			{
				return (NonPerformingCheck.Checked ? true : false);
			}
		}

		public bool CheckBoxVisible
		{
			set
			{
				NonPerformingCheck.Visible = value;
			}
		}


		public bool CheckBoxEnabled
		{
			set
			{
				NonPerformingCheck.Enabled = value;
			}
		}


		public bool ProceedButtonVisible
		{
			set
			{
				btnProceed.Visible = value;
			}
		}

		public event EventHandler onProceedButtonClicked;

		public event EventHandler onCancelButtonClicked;

		public event EventHandler onCheckBoxChecked;

		protected void NonPerformingCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (onCheckBoxChecked != null)
			{
				onCheckBoxChecked(sender, e);
			}
		}

		public void SetMTDInterest(Decimal amnt)
		{
			lblMonthToDateInterestAmt.Text = amnt.ToString(SAHL.Common.Constants.CurrencyFormat);
		}


		public void SetMTDInterestFixed(Decimal amnt)
		{
			lblMonthToDateInterestFixedAmt.Text = amnt.ToString(SAHL.Common.Constants.CurrencyFormat);
		}

		public void SetSuspendedInterestAmount(Decimal amnt)
		{
			lblSuspendedInterestAmt.Text = amnt.ToString(SAHL.Common.Constants.CurrencyFormat);
		}

		public void SetSuspendedInterestAmountFixed(Decimal amnt)
		{
			lblSuspendedInterestFixedAmount.Text = amnt.ToString(SAHL.Common.Constants.CurrencyFormat);
		}

		public void ShowFixedLeg()
		{
			headerFixed.Visible = true;
			tdSuspendedInterestFixed.Visible = true;
			tdMonthToDateInterestFixed.Visible = true;
		}

		public bool ShowConfirmWhenProceedClicked
		{
			set
			{
				if (value)
				{
					if (NonPerformingCheck.Checked)
						btnProceed.Attributes["onclick"] =
							"if(!confirm('Are you sure you want to Opt In to Non Performing?')) return false";
					else
						btnProceed.Attributes["onclick"] =
						"if(!confirm('Are you sure you want to Opt Out of Non Performing?')) return false";
				}
				else
				{
					btnProceed.Attributes["onclick"] = "return true";
				}
			}
		}
	}
}
