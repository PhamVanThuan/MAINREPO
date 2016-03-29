using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Text;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
	public class PaymentReceivedSummary : PaymentReceivedBase
	{
		public PaymentReceivedSummary(IPayment view, SAHLCommonBaseController controller)
			: base(view, controller)
		{

		}

		/// <summary>
		/// On View Initialized
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            _view.ReadOnly = true;
			_view.DisplayUpdatePanel = true;

			_view.BindForPaymentSummary(DebtCounselling, LoadDebitOrderDetails(DebtCounselling.Account.FinancialServices[0]));
        }
	}
}