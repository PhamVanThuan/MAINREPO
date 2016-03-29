using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using presenter = SAHL.Web.Views.DebtCounselling.Interfaces;
using models = SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
	/// <summary>
	/// Attorney View
	/// </summary>
	public class AttorneyView : AttorneyBase
	{
		/// <summary>
		/// Constructor for AttorneyUpdate
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public AttorneyView(presenter.IAttorney view, SAHLCommonBaseController controller)
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

			_view.Readonly = true;
		}
	}
}