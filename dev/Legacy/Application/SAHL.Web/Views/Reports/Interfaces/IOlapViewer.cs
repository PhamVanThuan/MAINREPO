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

namespace SAHL.Web.Views.Reports.Interfaces
{

    /// <summary>
    /// 
    /// </summary>
    public interface IOlapViewer : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        string ReportText { set;}
    }
}
