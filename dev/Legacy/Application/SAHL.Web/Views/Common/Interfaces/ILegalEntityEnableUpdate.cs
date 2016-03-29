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

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface ILegalEntityEnableUpdate : IViewBase
    {
        #region Interface Methods
        /// <summary>
        /// Sets the LabelMessage caption.
        /// </summary>
        /// <param name="LabelMessage"></param>
        void BindLabelMessage(string LabelMessage);

       /// <summary>
        /// Sets the LabelQuestion caption.
        /// </summary>
       /// <param name="LabelQuestion"></param>
        void BindLabelQuestion(string LabelQuestion);

        /// <summary>
        /// Sets the Cancel Button caption.
        /// </summary>
        /// <param name="CancelButtonText"></param>
        void BindCancelButtonText(string CancelButtonText);

        /// <summary>
        /// Sets the Submit Button caption.
        /// </summary>
        /// <param name="SubmitButtonText"></param>
        void BindSubmitButtonText(string SubmitButtonText);

        #endregion

        #region Interface Events
        /// <summary>
        /// Raised when the Submit button is clicked.
        /// </summary>
        event EventHandler OnSubmitButtonClick;

        /// <summary>
        /// Raised when the Submit button is clicked.
        /// </summary>
        event EventHandler OnCancelButtonClick;

        #endregion

    }
}
