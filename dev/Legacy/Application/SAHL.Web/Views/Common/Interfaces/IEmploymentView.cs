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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Web.Controls;
using SAHL.Web.Controls.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEmploymentView : IViewBase
    {
        #region Events

        /// <summary>
        /// Raised when the save button is clicked.
        /// </summary>
        event EventHandler SaveButtonClicked;
        /// <summary>
        /// Raised when the cancel button is clicked.
        /// </summary>
        event EventHandler CancelButtonClicked;
        /// <summary>
        /// Raised when Subsidy Details Buttons is clicked
        /// </summary>
        event EventHandler SubsidyDetailsClicked;
        /// <summary>
        /// Gets/sets the text on the Save button.
        /// </summary>
        string SaveButtonText { get; set; }
        /// <summary>
        /// Raised when Extended Details Buttons is clicked
        /// </summary>
        event EventHandler ExtendedDetailsClicked;
        /// <summary>
        /// Event signifying that the selected employment on the view has changed.
        /// </summary>
        event EventHandler EmploymentSelected;

        #endregion

        #region Properties

        /// <summary>
        /// Determines whether clicks on the grid causes postbacks or not.
        /// </summary>
        bool GridPostBack { set; }

        /// <summary>
        /// Gets/sets whether the legal entity column on the grid is visible.
        /// </summary>
        bool GridColumnLegalEntityVisible { get; set; }

        /// <summary>
        /// Gets/sets whether the start date column on the grid is visible.
        /// </summary>
        bool GridColumnStartDateVisible { get; set; }

        /// <summary>
        /// Toggles the visibility of the Save button.
        /// </summary>
        bool SaveButtonVisible { set; }

        /// <summary>
        /// Toggles the visibility of the Cancel button.
        /// </summary>
        bool CancelButtonVisible { set; }

        /// <summary>
        /// Toggles the visibility of the ExtendedDetails button.
        /// </summary>
        bool ExtendedDetailsButtonVisible { set; }

        /// <summary>
        /// Toggles the visibility of the SubsidyDetails button.
        /// </summary>
        bool SubsidyDetailsButtonVisible { set; }

        /// <summary>
        /// Gets/sets the employment record currently selected in the grid.  If the grid is display only (e.g. during an 
        /// add, this will return a null.
        /// </summary>
        IEmployment SelectedEmployment { get; set; }

        /// <summary>
        /// Gets a reference to the control displaying employment details.
        /// </summary>
        IEmploymentDetails EmploymentDetails { get; }

        /// <summary>
        /// Gets a reference to the control displaying employer details.
        /// </summary>
        IEmployerDetails EmployerDetails { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Binds employment details to the grid.  
        /// </summary>
        /// <param name="employmentDetails"></param>
        /// <param name="showPrevious">Whether to show previous employment records.</param>
        void BindEmploymentDetails(IEventList<IEmployment> employmentDetails, bool showPrevious);

        /// <summary>
        /// Populates an employment object with values entered in the form.
        /// </summary>
        IEmployment GetCapturedEmployment(IEmployment employment);

        /// <summary>
        /// Populates a NEW employment object (of the correct type) with values entered in the form.
        /// </summary>
        IEmployment GetCapturedEmployment();

        #endregion

    }
}
