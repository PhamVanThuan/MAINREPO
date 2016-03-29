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
using SAHL.Common.Security;

namespace SAHL.Web
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected static string CurrentUser
        {
            get
            {
                SAHLPrincipal p = SAHLPrincipal.GetCurrent();
                if (p == null)
                    return String.Empty;
                else
                    return p.Identity.Name;
            }
        }
    }
}
