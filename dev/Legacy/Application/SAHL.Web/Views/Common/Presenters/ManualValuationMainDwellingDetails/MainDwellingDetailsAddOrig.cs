using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.ManualValuationMainDwellingDetails
{
    public class MainDwellingDetailsAddOrig : MainDwellingDetailsAdd
    {
        public MainDwellingDetailsAddOrig(IValuationManualPropertyDetailsView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;
        }

        protected override void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            GlobalCacheData.Remove(ViewConstants.ValuationManual);
            base.X2Service.CancelActivity(_view.CurrentPrincipal);
            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);

        }
    }
}
