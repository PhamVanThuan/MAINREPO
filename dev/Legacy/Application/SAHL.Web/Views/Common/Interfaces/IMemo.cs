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
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMemo : IViewBase
    {
        # region Properties

        /// <summary>
        /// Show Controls for Display
        /// </summary>
        bool ShowControlsDisplay { set;}

        /// <summary>
        /// Show Controls for Update
        /// </summary>
        bool ShowControlsUpdate { set;}
        
        /// <summary>
        /// Show Controls for Add
        /// </summary>
        bool ShowControlsAdd { set;}      

        /// <summary>
        /// Show Buttons
        /// </summary>
        bool ShowButtons { set;}

        /// <summary>
        /// Gets the currently selected status
        /// </summary>
        int MemoStatusSelectedValue { get;set;}

       #endregion

        #region methods

        /// <summary>
        /// Set label text
        /// </summary>
        /// <param name="presenter"></param>
        /// <param name="action"></param>
        void SetLabelData(string presenter,string action);

       /// <summary>
        /// Bind Memo Grid
       /// </summary>
       /// <param name="memoLst"></param>
       /// <param name="action"></param>
        void BindMemoGrid(IEventList<SAHL.Common.BusinessModel.Interfaces.IMemo> memoLst,string action);

        /// <summary>
        /// Populate Status drop down
        /// </summary>
        void PopulateStatusDropDown();

        /// <summary>
        /// Populate status drop down for Update Mode
        /// </summary>
        void PopulateStatusUpdateDropDown();

        /// <summary>
        /// Set default Date for Add
        /// </summary>
        void SetDefaultDateForAdd();

        /// <summary>
        /// Bind memo fields
        /// </summary>
        /// <param name="memo"></param>
        void BindMemoFields(SAHL.Common.BusinessModel.Interfaces.IMemo memo);

        /// <summary>
        /// Set Grid Postback type
        /// </summary>
        void SetGridPostBackType();

        /// <summary>
        /// Get Captured memo record - used in Save
        /// </summary>
        /// <returns></returns>
        SAHL.Common.BusinessModel.Interfaces.IMemo GetCapturedMemo(SAHL.Common.BusinessModel.Interfaces.IMemo memo);

        /// <summary>
        /// Get Updated memo record - Save
        /// </summary>
        /// <param name="memo"></param>
        /// <returns></returns>
        SAHL.Common.BusinessModel.Interfaces.IMemo GetUpdatedMemo(SAHL.Common.BusinessModel.Interfaces.IMemo memo);

        /// <summary>
        /// Get updated memo record without followup (used for lead memo update)
        /// </summary>
        /// <param name="memo"></param>
        /// <returns></returns>
        SAHL.Common.BusinessModel.Interfaces.IMemo GetUpdatedMemoWithoutFollowup(SAHL.Common.BusinessModel.Interfaces.IMemo memo);


        /// <summary>
        /// Get Updated Memo for Follow Up - Add
        /// </summary>
        /// <param name="memo"></param>
        /// <returns></returns>
        SAHL.Common.BusinessModel.Interfaces.IMemo GetCapturedMemoForFollowupAdd(SAHL.Common.BusinessModel.Interfaces.IMemo memo);
        /// <summary>
        /// Get Updated memo for Follow Up Update
        /// </summary>
        /// <param name="memo"></param>
        /// <returns></returns>
        SAHL.Common.BusinessModel.Interfaces.IMemo GetUpdatedMemoForFollowUp(SAHL.Common.BusinessModel.Interfaces.IMemo memo);
        /// <summary>
        /// Bind memo grid for Display and Update - sets Grid selected index
        /// </summary>
        /// <param name="memoLst"></param>
        /// <param name="selectedIndex"></param>
        void BindMemoGridForDisplayAndUpdate(IEventList<SAHL.Common.BusinessModel.Interfaces.IMemo> memoLst, int selectedIndex);
        /// <summary>
        /// Bind Memo Grid for Follow Up
        /// </summary>
        /// <param name="memoLst"></param>
        /// <param name="selectedIndex"></param>
        void BindMemoGridForFollowUp(IEventList<SAHL.Common.BusinessModel.Interfaces.IMemo> memoLst, int selectedIndex);
        /// <summary>
        /// Hide expiry date - used in Follow Up Memo presnters
        /// </summary>
        void HideExpiryDateControlForFollowUp();
        /// <summary>
        /// Bind memo fields for follow up - Update
        /// </summary>
        /// <param name="memo"></param>
        void BindMemoFieldsForFollowUpUpdate(SAHL.Common.BusinessModel.Interfaces.IMemo memo);
        /// <summary>
        /// Hide Reminder Date - not needed for application add
        /// </summary>
        void HideReminderDate();

        void ShowHours();

        void HideUpdateButton();

        SAHL.Common.BusinessModel.Interfaces.IMemo GetCapturedMemoAddWithoutFollowup(SAHL.Common.BusinessModel.Interfaces.IMemo memo);
       
#endregion

        #region EventHandlers

        /// <summary>
        /// Memo Status Change event
        /// </summary>
        event KeyChangedEventHandler OnMemoStatusChanged;

        /// <summary>
        /// Selected Index Change on Grid
        /// </summary>
        event KeyChangedEventHandler OnMemoGridsSelectedIndexChanged;

        /// <summary>
        /// Raised when the add button is clicked.
        /// </summary>
        event EventHandler AddButtonClicked;
        /// <summary>
        /// Raised when the Update button is clicked
        /// </summary>
        event KeyChangedEventHandler UpdateButtonClicked;

        /// <summary>
        /// Raised when the cancel button is clicked.
        /// </summary>
        event EventHandler CancelButtonClicked;

        #endregion



    }
}
