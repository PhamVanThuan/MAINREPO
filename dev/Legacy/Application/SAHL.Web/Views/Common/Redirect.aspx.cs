using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Common
{
    public partial class Redirect : SAHLCommonBaseView, IRedirect
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            Response.Clear();
        }
    }
}
