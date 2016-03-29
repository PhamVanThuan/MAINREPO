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


namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApplicationSummary : IViewBase
    {
        #region Events
        /// <summary>
        /// Raised when the cancel button is clicked
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// Raised when the Transition History button is clicked
        /// </summary>
        event EventHandler OnTransitionHistoryClicked;

        ///// <summary>
        ///// Fires when the selected index of the summary grid is changed
        ///// </summary>
        //event KeyChangedEventHandler OnSummaryGridSelectedIndexChanged;

        #endregion

        #region Properties
        /// <summary>
        /// Set whether the cancel button must be shown
        /// </summary>
        bool ShowCancelButton { set;}

        /// <summary>
        /// Set whether the cancel button must be shown
        /// </summary>
        bool ShowHistoryButton { set;}

        /// <summary>
        /// Sets the account to be used by the view
        /// </summary>
        IApplication application { set;}

        /// <summary>
        /// Set whether Capitec details must be shown
        /// </summary>
        bool ShowCapitecDetails { set; }

        bool ShowComcorpDetails { set; }

        bool ShowIsGEPFDetails { set; }

        bool ShowStopOrderDiscountEligibility { set; }
        #endregion

        #region methods

        /// <summary>
        /// 
        /// </summary>
        void BindControls();



        #endregion

    }
}
