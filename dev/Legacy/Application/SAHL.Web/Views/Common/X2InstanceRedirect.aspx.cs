using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class X2InstanceRedirect : SAHLCommonBaseView, IX2InstanceRedirect
    {

        public string MessageText
        {
            set { this.Label1.Text = value; }
        }

        public bool DisplayRefreshButton
        {
            set { this.btnRefresh.Visible = value; }
        }

    }
}
