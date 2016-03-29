using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Web.UI;
namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    public class CourtDetailsView : CourtDetailsBase
    {
        /// <summary>
		/// Constructor for CourtDetailsView
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
        public CourtDetailsView(ICourtDetails view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
		}

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.ShowMaintenancePanel = false;
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;
        }
    }
}