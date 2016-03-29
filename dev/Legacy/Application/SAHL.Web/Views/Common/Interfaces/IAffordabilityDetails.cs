using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Models.Affordability;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface IAffordabilityDetails : IViewBase
    {
        #region Properties

        /// <summary>
        /// Sets the legal entity variable
        /// </summary>
        ILegalEntity LegalEntity { set; }

        IEnumerable<AffordabilityModel> Affordability { get; set; }

        ///// <summary>
        /////
        ///// </summary>
        //IApplication Application { set;}

        /// <summary>
        /// Sets whether the cancel and submit button must be visible
        /// </summary>
        bool ShowButtons { set; }

        /// <summary>
        /// Sets whether the controls on the page are readonly
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        bool ReadOnly { set; }

        /// <summary>
        /// Sets the text to be shown on the submit button
        /// </summary>
        string SubmitButtonText { set; }

        /// <summary>
        /// GEt or Set The Application Object
        /// </summary>
        IApplication application { get; set; }

        /// <summary>
        ///
        /// </summary>
        string NumberOfDependantsInHouseHold { set; }

        /// <summary>
        ///
        /// </summary>
        string ContributingDependants { set; }

        /// <summary>
        ///
        /// </summary>
        string SelectedNumDependantsInHousehold { get; }

        /// <summary>
        ///
        /// </summary>
        string SelectedNumContributingDependants { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Binds the expense data to the expense controls
        /// </summary>
        void BindControls();

        #endregion Methods

        #region Events

        /// <summary>
        /// Event handler for when the Cancel button on the page is clicked
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        ///
        /// </summary>
        event EventHandler OnSubmitButtonClicked;
        //event KeyChangedEventHandler OnSubmitButtonClicked;

        #endregion Events
    }
}