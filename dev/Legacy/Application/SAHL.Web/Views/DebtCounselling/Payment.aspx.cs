using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.DebtCounselling
{
	public partial class Payment : SAHLCommonBaseView, IPayment
	{
		public bool EnableButtons
		{
			set
			{
				tblButton.Visible = false;
			}
		}

		public event EventHandler OnUpdateButtonClicked;
		public event EventHandler OnCancelButtonClicked;

		public IDebtCounselling DebtCounselling { get; set; }

		public bool ReadOnly { get; set; }

		public bool DisplayUpdatePanel
		{
			set
			{
				trPmtRecDt.Visible = value;
				trPmtRecAmt.Visible = value;
				trTermReviewDt.Visible = value;
				trInstExpDt.Visible = value;
				trPDA.Visible = value;
			}
		}
		public bool DisplayUpdateReviewDatePanel 
		{
			set
			{
				trTermReviewDt.Visible = value;
			}
		}

		/// <summary>
		/// On Load
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			SetUpView();
		}

		private void SetUpView()
		{
			// Editable Details
			dtePaymentReceivedDate.Visible = !ReadOnly;
			txtPaymentReceivedAmount.Visible = !ReadOnly;
			dteTermReviewDate.Visible = !ReadOnly;

			// Summary Details
			lblPaymentReceivedDate.Visible = ReadOnly;
			lblPaymentReceivedAmount.Visible = ReadOnly;
			lblTermReviewDate.Visible = ReadOnly;

			//Buttons Visiblity
			tblButton.Visible = !ReadOnly;
		}

		/// <summary>
		/// Bind Readonly Details
		/// </summary>
		/// <param name="debtCounsellingCase"></param>
		/// <param name="debitOrderDetails"></param>
		private void BindReadonlyDetails(IList<SAHL.Web.Views.DebtCounselling.Presenters.PaymentReceivedSummary.DebitOrderDetail> debitOrderDetails)
		{
			//Instalment Expectancy Date
			lblResetInstallmentExpectancyDate.Text = String.Empty;
			foreach (var debitOrderDetail in debitOrderDetails)
			{
				lblResetInstallmentExpectancyDate.Text += String.Format("<b>Effective Date :</b> {0}, <b>Debit Order Day :</b> {1}<br/>", debitOrderDetail.EffectiveDate, debitOrderDetail.DebitOrderDay);
			}

			//PDA
			lblPaymentDistributionAgent.Text = (DebtCounselling.PaymentDistributionAgent != null ? DebtCounselling.PaymentDistributionAgent.DisplayName : "-");
		}

		/// <summary>
		/// Bind Payment Summary
		/// </summary>
		/// <param name="debtCounsellingCase"></param>
		/// <param name="debitOrderDetails"></param>
		public void BindForPaymentSummary(IDebtCounselling debtCounsellingCase, IList<SAHL.Web.Views.DebtCounselling.Presenters.PaymentReceivedSummary.DebitOrderDetail> debitOrderDetails)
		{
			DebtCounselling = debtCounsellingCase;

			BindReadonlyDetails(debitOrderDetails);

			//Payment Received Date
			lblPaymentReceivedDate.Text = DebtCounselling.PaymentReceivedDate.HasValue ? DebtCounselling.PaymentReceivedDate.Value.ToShortDateString() : "-";

			//Payment Received Amount
			lblPaymentReceivedAmount.Text = DebtCounselling.PaymentReceivedAmount.HasValue ? DebtCounselling.PaymentReceivedAmount.Value.ToString() : "-";

			//Term Review Date
            lblTermReviewDate.Text = DebtCounselling.AcceptedActiveProposal != null &&
                                     DebtCounselling.AcceptedActiveProposal.ReviewDate.HasValue
                                     ? DebtCounselling.AcceptedActiveProposal.ReviewDate.Value.ToShortDateString()
									 : "-";
		}

		/// <summary>
		/// Bind Payment Received Details
		/// </summary>
		/// <param name="debtCounsellingCase"></param>
		/// <param name="debitOrderDetails"></param>
		public void BindForPaymentUpdate(IDebtCounselling debtCounsellingCase, IList<SAHL.Web.Views.DebtCounselling.Presenters.PaymentReceivedSummary.DebitOrderDetail> debitOrderDetails)
		{
			DebtCounselling = debtCounsellingCase;

			BindReadonlyDetails(debitOrderDetails);

			///Only bind the initial values if it's not a post back
			if (!IsPostBack)
			{
				//Payment Received Date
				dtePaymentReceivedDate.Date = DebtCounselling.PaymentReceivedDate != null ? DebtCounselling.PaymentReceivedDate : DateTime.Now;

				//Payment Received Amount
				txtPaymentReceivedAmount.Amount = DebtCounselling.PaymentReceivedAmount;

				//Term Review Date
				//If the Accepted Proposal is not null, and we don't expect it to be null here
                if (DebtCounselling.AcceptedActiveProposal != null)
				{
                    dteTermReviewDate.Date = DebtCounselling.AcceptedActiveProposal.ReviewDate;
				}
			}
		}

		/// <summary>
		/// On Update Click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void btnUpdate_Click(object sender, EventArgs e)
		{
			DebtCounselling.PaymentReceivedDate = dtePaymentReceivedDate.Date;
			DebtCounselling.PaymentReceivedAmount = txtPaymentReceivedAmount.Amount;
            if (DebtCounselling.AcceptedActiveProposal != null)
			{
                DebtCounselling.AcceptedActiveProposal.ReviewDate = dteTermReviewDate.Date;
			}

			if (OnUpdateButtonClicked != null)
				OnUpdateButtonClicked(sender, e);
		}

		/// <summary>
		/// On Cancel Click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void btnCancel_Click(object sender, EventArgs e)
		{
			if (OnCancelButtonClicked != null)
				OnCancelButtonClicked(sender, e);
		}
	}
}