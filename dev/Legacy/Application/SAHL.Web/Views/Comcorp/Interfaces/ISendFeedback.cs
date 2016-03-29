using SAHL.Common.Web.UI;
using System;

namespace SAHL.Web.Views.Comcorp.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface ISendFeedback : IViewBase
    {
        #region properties

        string EventComment { get; }

        bool SubmitButtonEnabled { set; }

        #endregion properties

        #region EventHandlers

        /// <summary>
        /// Raised when the Submit button is clicked
        /// </summary>
        event EventHandler SubmitButtonClicked;

        /// <summary>
        /// Raised when the cancel button is clicked.
        /// </summary>
        event EventHandler CancelButtonClicked;

        #endregion EventHandlers
    }
}