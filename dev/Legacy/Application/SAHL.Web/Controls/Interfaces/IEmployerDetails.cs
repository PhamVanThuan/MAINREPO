using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Controls.Interfaces
{
    /// <summary>
    /// Interface to enable mocking of the <see cref="EmployerDetails"/> control
    /// </summary>
    public interface IEmployerDetails
    {
        /// <summary>
        /// Gets/sets the <see cref="IEmployer"/> displayed on the control.
        /// </summary>
        IEmployer Employer { get; set; }

        /// <summary>
        /// Gets the key of the selected employer, if there is one.
        /// </summary>
        int? EmployerKey { get; }

        /// <summary>
        /// Clears employment details.
        /// </summary>
        void ClearEmployer();

        /// <summary>
        /// Gets/sets the visibility of the control.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Gets/sets the current edit mode of the control.  This defaults to <see cref="EmployerDetailsEditMode.ReadOnly"/>.
        /// </summary>
        EmployerDetailsEditMode EditMode { get; set; }

        /// <summary>
        /// Event that occurs when an employer is selected from the autocomplete list.
        /// </summary>
        event KeyChangedEventHandler EmployerSelected;

    }
}
