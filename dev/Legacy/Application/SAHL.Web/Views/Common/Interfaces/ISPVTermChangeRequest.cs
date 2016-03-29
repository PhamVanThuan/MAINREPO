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
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Collections;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISPVTermChangeRequest : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        double? CurrentBalance { set;get;}
        /// <summary>
        /// 
        /// </summary>
        int? LoanNumber { set;get;}
        /// <summary>
        /// 
        /// </summary>
        double? LoanAmount { set;get;}
        /// <summary>
        /// 
        /// </summary>
        string CurrentSPV { set;get;}
        /// <summary>
        /// 
        /// </summary>
        string Requested { set;get;}
        /// <summary>
        /// 
        /// </summary>
        void BindControls();
        /// <summary>
        /// 
        /// </summary>
        string SPV { set;get;}
        /// <summary>
        /// 
        /// </summary>
        int? InitialTerm { set;get;}
        /// <summary>
        /// 
        /// </summary>
        int? NewTerm { set;get;}
        /// <summary>
        /// 
        /// </summary>
        double? NewInstallment { set;get;}
        /// <summary>
        /// 
        /// </summary>
        int? CurrentTerm { set;get;}
        /// <summary>
        /// 
        /// </summary>
        double? LTV { set;get;}
        /// <summary>
        /// 
        /// </summary>
        double? CurrentPTI { set;get;}
        /// <summary>
        /// 
        /// </summary>
        double? NewPTI { set;get;}
    }
}
