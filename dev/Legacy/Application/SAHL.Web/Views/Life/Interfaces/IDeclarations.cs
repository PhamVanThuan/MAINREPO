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
    public interface IDeclarations : IViewBase
    {
        /// <summary>
        ///Event that is raised when then Next button is clicked
        /// </summary>
        event EventHandler OnNextButtonClicked;

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

        /// <summary>
        /// Binds the Declarations text statements
        /// </summary>
        /// <param name="textItems">A collection of ITextStatements</param>
        void BindDeclarations(IReadOnlyEventList<ITextStatement> textItems);
    }
}
