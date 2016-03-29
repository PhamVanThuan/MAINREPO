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

    /// <summary>
    /// 
    /// </summary>
    public interface IReportingServicesReport : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        string ReportPath { set;}

        /// <summary>
        /// 
        /// </summary>
        IDictionary<IReportParameter, object> parameters { set;}
    }
}
