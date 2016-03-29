using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.DebtCounselling.Interfaces
{
	public interface IPayment : IViewBase
	{
		bool DisplayUpdatePanel { set; }
		bool DisplayUpdateReviewDatePanel { set; }

		/// <summary>
		/// Enable Buttons
		/// </summary>
		bool EnableButtons { set; }

		/// <summary>
		/// 
		/// </summary>
		IDebtCounselling DebtCounselling { set; get; }

		/// <summary>
		/// 
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        bool ReadOnly { set; }

		/// <summary>
		/// Bind for Payment Summary
		/// </summary>
		/// <param name="debtCounsellingCase"></param>
		/// <param name="debitOrderDetails"></param>
		void BindForPaymentSummary(IDebtCounselling debtCounsellingCase, IList<SAHL.Web.Views.DebtCounselling.Presenters.PaymentReceivedSummary.DebitOrderDetail> debitOrderDetails);

		/// <summary>
		/// Bind for Payment Update
		/// </summary>
		void BindForPaymentUpdate(IDebtCounselling debtCounsellingCase, IList<SAHL.Web.Views.DebtCounselling.Presenters.PaymentReceivedSummary.DebitOrderDetail> debitOrderDetails);

		/// <summary>
		/// 
		/// </summary>
		event EventHandler OnUpdateButtonClicked;

		/// <summary>
		/// 
		/// </summary>
		event EventHandler OnCancelButtonClicked;
	}
}