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
using Microsoft.ApplicationBlocks.UIProcess;

namespace SAHL.Web.Views.Common.Presenters
{
    public class X2WorkFlowSummary : SAHLCommonBasePresenter<IX2WorkFlowSummary>
    {

        public X2WorkFlowSummary(IX2WorkFlowSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }


    }
}
