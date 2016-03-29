using System;
using System.Data;
using System.Text;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common;
using SAHL.Common.Web.UI;
using SAHL.Common.Authentication;
using SAHL.Web.UI;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class X2TaskSummary : SAHLCommonBaseView, IX2TaskSummary
    {
        #region Private Variables
        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage())
                return;

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage())
                return;
        }

    }
}
