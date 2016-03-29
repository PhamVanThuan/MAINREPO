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
using SAHL.Web.Controls;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Administration.Interfaces
{

    /// <summary>
    /// Defines the interface for view displaying employer information.
    /// </summary>
    public interface IEmployer : IViewBase
    {

        #region Events

        /// <summary>
        /// Raised when the add button is clicked.
        /// </summary>
        event EventHandler AddButtonClicked;

        /// <summary>
        /// Raised when the cancel button is clicked.
        /// </summary>
        event EventHandler CancelButtonClicked;

        /// <summary>
        /// Raised when the "clear form" button is clicked.
        /// </summary>
        event EventHandler ClearFormButtonClicked;

        /// <summary>
        /// Raised when the update button is clicked.
        /// </summary>
        event EventHandler UpdateButtonClicked;

        /// <summary>
        /// Raised when an employer is selected.
        /// </summary>
        event KeyChangedEventHandler EmployerSelected;

        #endregion

        #region Properties 

        /// <summary>
        /// Sets whether the add button should be displayed.
        /// </summary>
        bool AddButtonVisible { set; }

        /// <summary>
        /// Sets whether the cancel button should be displayed.
        /// </summary>
        bool CancelButtonVisible { set; }

        /// <summary>
        /// Sets whether the "Clear Form" button should be displayed.
        /// </summary>
        bool ClearFormButtonVisible { set; }

        /// <summary>
        /// Sets whether the "Clear Form" button should be enabled.
        /// </summary>
        bool ClearFormButtonEnabled { set; }

        /// <summary>
        /// Sets whether the updated button should be enabled.
        /// </summary>
        bool UpdateButtonEnabled { set; }

        /// <summary>
        /// Sets whether the update button should be displayed.
        /// </summary>
        bool UpdateButtonVisible { set; }

        /// <summary>
        /// Gets/sets the edit mode of the <see cref="EmployerDetails"/> control in the view.
        /// </summary>
        EmployerDetailsEditMode EditMode { get; set; }

        /// <summary>
        /// Gets a populated IEmployer object from the view, or sets the values populated on the view.  The object will reflect the 
        /// values on the form, so in an update this can be relied on to be the values that should be persisted to the database.
        /// </summary>
        /// <returns>A populated IEmployer entity.</returns>
        SAHL.Common.BusinessModel.Interfaces.IEmployer SelectedEmployer { get; set; }


        #endregion

        #region Methods

        /// <summary>
        /// Clears the form and removes the selected employer from the control.
        /// </summary>
        void ClearEmployer();

        #endregion

    }
}
