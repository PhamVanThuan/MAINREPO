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
using SAHL.Web.Views.Reports.Interfaces;
using System.IO;

namespace SAHL.Web.Views.Reports
{
    public partial class PDFReportViewer : SAHLCommonBaseView, IPDFReportViewer
    {
        private string _pdfReportPath;

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

            this.pdfViewer.FilePath = _pdfReportPath;
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            if (onCancelButtonClicked != null)
                onCancelButtonClicked(sender, e);
        }

        #region IPDFReportViewer Members

        public event EventHandler onCancelButtonClicked;

        public string PDFReportPath
        {
            get
            {
                return _pdfReportPath;
            }
            set
            {
                _pdfReportPath = value;
            }
        }

        public bool Cancelled
        {
            get
            {
                if (Request.Form["__EVENTTARGET"] != null)
                {
                    if (Request.Form["__EVENTTARGET"] == btnCancel.UniqueID)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion
    }
}
