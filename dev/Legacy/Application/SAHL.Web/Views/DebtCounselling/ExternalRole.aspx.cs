using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using businessModel = SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.DebtCounselling
{
	/// <summary>
	/// External Role View
	/// </summary>
	public partial class ExternalRole : SAHLCommonBaseView, IExternalRole
	{
		public businessModel.IDebtCounselling DebtCounsellingCase { get; set; }

		/// <summary>
		/// Page Load
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			if (DebtCounsellingCase != null)
			{
				lblSelectedAttorney.Text = DebtCounsellingCase.LitigationAttorney != null ? DebtCounsellingCase.LitigationAttorney.LegalEntity.DisplayName : "-";
				lblSelectedDebtCounsellor.Text = DebtCounsellingCase.DebtCounsellor != null ? DebtCounsellingCase.DebtCounsellor.DisplayName : "-";
				lblSelectedPDA.Text = DebtCounsellingCase.PaymentDistributionAgent != null ? DebtCounsellingCase.PaymentDistributionAgent.DisplayName : "-";
			}
		}
	}
}