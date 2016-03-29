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
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Life.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFAIS : IViewBase
    {
        /// <summary>
        /// Event raised when next button is clicked
        /// </summary>
        event EventHandler OnNextButtonClicked;

        /// <summary>
        /// Get/Set whether Spouse Confirmation is required
        /// </summary>
        bool ConfirmationRequired { get; set;}

        /// <summary>
        /// Get/Set the Contact Number
        /// </summary>
        string ContactNumber { get; set;}

        /// <summary>
        /// Get/Set whether the view is in Confirmation Mode
        /// </summary>
        bool ConfirmationMode { get; set;}

        /// <summary>
        /// Get/Set whether the Activity has been completed
        /// </summary>
        bool ActivityDone { get; set;}

        /// <summary>
        /// Get/Set whether all the Checkboxes have been checked
        /// </summary>
        bool AllOptionsChecked { get; set;}

        bool ContactOptionError { get; set;}

        bool ContactNumberError { get; set;}

        int LifePolicyTypeKey { get; set;}

        /// <summary>
        /// Binds the FAIS text statements
        /// </summary>
        /// <param name="textItems">A collection of ITextStatements</param>
        void BindFAIS(IReadOnlyEventList<ITextStatement> textItems);

    }
}
