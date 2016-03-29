using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Origination.Presenters
{
    public class ApplicationWarnings : SAHLCommonBasePresenter<IApplicationWarnings>
    {
        public ApplicationWarnings(IApplicationWarnings view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

    }
}
