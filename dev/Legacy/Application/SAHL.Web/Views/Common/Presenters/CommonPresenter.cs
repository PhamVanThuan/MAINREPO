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

namespace SAHL.Web.Views.Common.Presenters
{
    public class CommonPresenter : SAHLCommonBasePresenter<IBlank>
    {
        public CommonPresenter(IBlank view, SAHLCommonBaseController controller) : base(view, controller) 
        { 
        }


    }
}
