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
    public class X2WorkFlowListSummary : SAHLCommonBasePresenter<IX2WorkFlowListSummary>
    {

        public X2WorkFlowListSummary(IX2WorkFlowListSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }


    }
}
