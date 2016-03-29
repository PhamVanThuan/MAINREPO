using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.UI;

using SAHL.Common;


namespace SAHL.Web.Views.Common.Presenters.ManualValuationMainDwellingDetails
{
    public class MainDwellingExtendedDisplayOrig : MainDwellingExtendedDisplay
    {
        public MainDwellingExtendedDisplayOrig(IValuationManualPropertyDetailsView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void _view_OnBackButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("Back");
        }

        protected override void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            GlobalCacheData.Remove(ViewConstants.ValuationManual);
            CBOMenuNode node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            if (node == null)
                _view.Navigator.Navigate("Cancel");
            else
            {
                base.X2Service.CancelActivity(_view.CurrentPrincipal);
                base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
            }
        }
    }
}
