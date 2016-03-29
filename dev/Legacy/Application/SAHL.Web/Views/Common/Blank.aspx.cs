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
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Common
{
    public partial class Blank : SAHLCommonBaseView, IBlank
    {
        public event EventHandler OnNextClick; 

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string Text
        {
            set
            {
                lblText.Text = value;
            }
        }
    }
}
