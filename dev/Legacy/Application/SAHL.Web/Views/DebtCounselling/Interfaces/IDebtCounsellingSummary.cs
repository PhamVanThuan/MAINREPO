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
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;


namespace SAHL.Web.Views.DebtCounselling.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDebtCounsellingSummary : IViewBase
    {
        #region Events

        #endregion

        #region Properties
        /// <summary>
        /// Set whether to display details from the case in the eWork Loss Control map
        /// </summary>
        bool DisplayEworkCaseDetails { set; }

        #endregion

        #region methods

        /// <summary>
        /// 
        /// </summary>
        void BindControls(IDebtCounselling debtCounselling);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stageName"></param>
        /// <param name="assignedUser"></param>
        void BindEworkCaseDetails(string stageName, IADUser assignedUser);

        #endregion

    }
}
