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
    public partial class Notification : SAHLCommonBaseView, INotification
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #region INotification Members

        /// <summary>
        /// This string is displayed as the notification message.
        /// </summary>
        public string NotificationText
        {
            get
            {
                return lblNotification.Text;
            }
            set
            {
                lblNotification.Text = value;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // replace any line breaks with HTML tags
            lblNotification.Text = lblNotification.Text.Replace(">", "gt;");
            lblNotification.Text = lblNotification.Text.Replace("<", "lt;");
            if (lblNotification.Text.IndexOf(Environment.NewLine) > -1)
            {
                string nl = "<li type=\"square\">";
                lblNotification.Text = nl + lblNotification.Text.Replace(Environment.NewLine, nl);
            }

        }

        #endregion
    }
}
