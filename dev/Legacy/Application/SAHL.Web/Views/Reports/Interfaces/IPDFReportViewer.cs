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
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Reports.Interfaces
{
    public interface IPDFReportViewer : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        string PDFReportPath { get; set;}   

        /// <summary>
        /// 
        /// </summary>
        event EventHandler onCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        bool Cancelled { get;}   
    }
}
