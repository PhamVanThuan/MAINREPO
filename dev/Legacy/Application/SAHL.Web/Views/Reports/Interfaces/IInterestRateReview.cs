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
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;

namespace SAHL.Web.Views.Reports.Interfaces
{

    /// <summary>
    /// 
    /// </summary>
    public interface IInterestRateReview : IViewBase
    {

        event EventHandler OnCancelButtonClicked;
        
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        ///// <summary>
        ///// 
        ///// </summary>
        //IReportStatement ReportStatement { set;}

        /// <summary>
        /// 
        /// </summary>
        IDictionary<IReportParameter, object> lstParameters { get;}
    }
}
