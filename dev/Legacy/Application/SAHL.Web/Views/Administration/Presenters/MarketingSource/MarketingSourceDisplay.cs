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
using SAHL.Web.Views.Administration.Interfaces;

namespace SAHL.Web.Views.Administration.Presenters.MarketingSource
{
    public class MarketingSourceDisplay : MarketingSourceBase
    {
         public MarketingSourceDisplay(IMarketingSource view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

                return;
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
         
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

           
        }

    }
}
