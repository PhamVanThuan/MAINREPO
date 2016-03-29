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

namespace SAHL.Web.Views
{
    /// <summary>
    /// Serves as a basic presenter for screens do not require any additional processing (for example, default screens).
    /// </summary>
    public class CommonPresenter : SAHLCommonBasePresenter<IViewBase>
    {
        public CommonPresenter(IViewBase view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

    }
}
