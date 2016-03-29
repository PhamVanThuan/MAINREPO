namespace SAHL.Web.Views.Origination
{
    using System;
    using System.Reflection;
    using System.Text;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.Web.UI;
    using SAHL.Web.Views.Origination.Interfaces;

    public partial class DisplayNetLeadXML : SAHLCommonBaseView, IDisplayNetLeadXML
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.ShouldRunPage)
                return;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!this.ShouldRunPage)
                return;
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnRetryCreateClicked;

        /// <summary>
        /// 
        /// </summary>
        protected void btnRetryCreate_Click(object sender, EventArgs e)
        {
            if (OnRetryCreateClicked != null)
                OnRetryCreateClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        public void BindRetryButtonText(string stateName)
        {
            if (!string.IsNullOrEmpty(stateName))
            {
                btnRetry.Text = "Retry " + stateName;
                btnRetry.Enabled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void BindRawNetLeadXML(ILeadInputInformation leadInputInformation)
        {
            if (leadInputInformation != null)
            {
                StringBuilder sb = new StringBuilder();

                PropertyInfo[] properties = leadInputInformation.GetType().GetProperties();
                foreach (PropertyInfo pi in properties)
                {
                    sb.Append(string.Format("{0} : {1}", pi.Name, pi.GetValue(leadInputInformation, null)));
                    sb.Append("<br />");
                }

                lbNetLeadXML.Text = sb.ToString();
            }
            else
                lbNetLeadXML.Text = "No NetLead XML data";
        }
    }
}