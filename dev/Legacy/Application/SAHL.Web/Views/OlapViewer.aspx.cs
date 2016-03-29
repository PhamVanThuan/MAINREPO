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
using Microsoft.Reporting.WebForms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Globalization;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Reports.Interfaces;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.Reports
{
    /// <summary>
    /// 
    /// </summary>
    public partial class OlapViewer : SAHLCommonBaseView,IOlapViewer
    {
        private string _reportText;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage) return;
        }
        

        #region IOlapViewer Members

        public event EventHandler OnSubmitButtonClicked;
      
        public string ReportText
        {
            set { _reportText = value; }
        }

        #endregion

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnSubmitButtonClicked(sender, e);
        }

        protected override void OnPreRender(EventArgs e)
        {            
            base.OnPreRender(e);

            ReportViewerFrame.Attributes["src"] = @"OLAPFrameContent.aspx" + "?codeSrc=" + _reportText;

        }
    }
}