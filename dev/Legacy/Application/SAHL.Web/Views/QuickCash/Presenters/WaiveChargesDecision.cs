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
using SAHL.Web.Views.QuickCash.Interfaces;

namespace SAHL.Web.Views.QuickCash.Presenters
{
    public class WaiveChargesDecision : SAHLCommonBasePresenter<IWaiveCharges>
    {
        public WaiveChargesDecision(IWaiveCharges View, SAHLCommonBaseController Controller)
            : base(View, Controller)
        {
        }
    }
}
