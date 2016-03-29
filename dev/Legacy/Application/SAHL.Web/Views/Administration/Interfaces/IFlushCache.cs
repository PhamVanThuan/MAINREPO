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

namespace SAHL.Web.Views.Administration.Interfaces
{
    /// <summary>
    /// View used to flush various items from the cache.
    /// </summary>
    public interface IFlushCache : IViewBase
    {
        /// <summary>
        /// Raised when the lookups button is clicked.
        /// </summary>
        event KeyChangedEventHandler LookupButtonClicked;

        /// <summary>
        /// Raised when the 'Clear All' lookups button is clicked.
        /// </summary>
        event EventHandler LookupAllButtonClicked;

        /// <summary>
        /// Raised when the 'Clear All' UIStatements button is clicked.
        /// </summary>
        event EventHandler UIStatementButtonClicked;

		/// <summary>
		/// Raised when the 'Clear Org Structure' button is clicked.
		/// </summary>
		event EventHandler OrgStructureButtonClicked;

        event EventHandler RuleItemButtonClicked;

        /// <summary>
        /// Raised when the 'Clear All' button is clicked.
        /// </summary>
        event EventHandler ClearAllButtonClicked;

        /// <summary>
        /// Raised when the user access button is clicked.
        /// </summary>
        event KeyChangedEventHandler UserAccessButtonClicked;

        /// <summary>
        /// Displays an info message on the screen to the user.
        /// </summary>
        /// <param name="text">The text to display.</param>
        /// <param name="isError">Whether the message is an error or not.</param>
        void SetMessage(string text, bool isError);

    }
}
