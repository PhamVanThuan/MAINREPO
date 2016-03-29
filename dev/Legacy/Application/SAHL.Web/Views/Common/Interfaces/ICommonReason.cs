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
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommonReason : IViewBase
    {
        #region Events

        /// <summary>
        /// An event for handling when the SubmitButton is clicked
        /// </summary>
        event KeyChangedEventHandler OnSubmitButtonClicked;
        /// <summary>
        /// An event for handling when the Cancel is clicked
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reasonTypes"></param>
        void BindReasonTypes(List<IReasonType> reasonTypes);

        void BindReasonHistoryGrid(IReadOnlyEventList<IReason> reasons);

        #endregion

        #region Properties

        #region Get Properties

        //string Comment { get;}
        
        //int ReasonDefinitionKey { get;}

        #endregion

        bool SortByReasonDescription { get; set; }

        #region Set Properties
        /// <summary>
        /// Set whether the memo panel should be visible
        /// </summary>
        bool MemoPanelVisible { set;}

        /// <summary>
        /// Set whether the cancel button should be visible
        /// </summary>
        bool CancelButtonVisible { set;}

        /// <summary>
        /// Submit Button Visible
        /// </summary>
        bool SubmitButtonVisible { set; }

        /// <summary>
        /// 
        /// </summary>
        bool OnlyOneReasonCanBeSelected { set; }

        bool ShowHistoryPanel { set; }

        bool ShowUpdatePanel { set;}

        string SetHistoryPanelGroupingText { set;}

        bool HistoryDisplay { set;}

        /// <summary>
        /// Set HiddenInd Text - indicates whether or not the confirm button click event allows the user
        /// to continue without selecting a reason
        /// </summary>
        string SetHiddenIndText { set;}

        /// <summary>
        /// Set the text of the submit button to something other than submit.
        /// </summary>
        string SubmitButtonText
        {
            set;
        }

        /// <summary>
        /// Cancel Button Text
        /// </summary>
        string CancelButtonText
        {
            set;
        }

        /// <summary>
        /// When setting the show update panel to false, the buttons are hidden
        /// Set this to true to show the buttons when not using teh panel if required.
        /// </summary>
        bool ShowSubmitButtons
        { set; }

        #endregion

        #endregion

    }
}
