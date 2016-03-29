using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.QuickCash.Interfaces;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.QuickCash.Presenters
{
    public class QuickCashThirdPartyPayments : SAHLCommonBasePresenter<IQuickCashThirdPartyPayments>
    {

        public QuickCashThirdPartyPayments(IQuickCashThirdPartyPayments View, SAHLCommonBaseController Controller)
            : base(View, Controller)
        {
        }
    }
}
