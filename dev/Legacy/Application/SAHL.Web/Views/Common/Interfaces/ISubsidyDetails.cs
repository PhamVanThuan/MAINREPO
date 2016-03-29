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
using System.Collections.Generic;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// Defines the contract for a Subsidy Details view.
    /// </summary>
    public interface ISubsidyDetails : IViewBase
    {
        #region Events

        /// <summary>
        /// Event signifying that the back button has been clicked.
        /// </summary>
        event EventHandler BackButtonClicked;

        /// <summary>
        /// Event signifying that the cancel button has been clicked.
        /// </summary>
        event EventHandler CancelButtonClicked;

        /// <summary>
        /// Event signifying that the selected subsidy on the view has changed.
        /// </summary>
        event KeyChangedEventHandler SubsidySelected;

        /// <summary>
        /// Event signifying that the save button has been clicked.
        /// </summary>
        event EventHandler SaveButtonClicked;
       

        #endregion


        #region Properties

        /// <summary>
        /// Gets the subsidy captured in the input form.  This will not return an entity that is ready to be 
        /// persisted.  Only the following properties will be populated: Accounts, Notch, Paypoint, Rank, 
        /// SalaryNumber, StopOrderAmount, SubsidyProvider.
        /// </summary>
        ISubsidy GetCapturedSubsidy();

        /// <summary>
        /// Populates <c>subsidy</c> with the values entered in the form.
        /// </summary>
        /// <param name="subsidy"></param>
        /// <returns></returns>
        ISubsidy GetCapturedSubsidy(ISubsidy subsidy);

        /// <summary>
        /// Gets/sets the visible status of the subsidy grid.
        /// </summary>
        bool GridVisible { get; set; }

        /// <summary>
        /// Gets/sets whether the screen is read-only or not.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        bool ReadOnly { get; set; }

        /// <summary>
        /// Show Navigation buttons
        /// </summary>
        bool ShowButtons { set; }
        /// <summary>
        /// Show Status Control
        /// </summary>
        bool ShowStatus { set;}

        /// <summary>
        /// Gets/sets the text on the save button.
        /// </summary>
        string SaveButtonText { get; set; }

        /// <summary>
        /// Sets the subsidy details to display on the screen.
        /// </summary>
        /// <param name="subsidyKey"></param>
        void SetSubsidy(int subsidyKey);

        int EmploymentStatusKey { get; set;}

        #endregion

        #region Methods

        /// <summary>
        /// Binds a list of accounts and applications to the view for selection for the subsidy.
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="applications"></param>
        void BindAccounts(IEventList<IAccount> accounts, IEventList<IApplication> applications);

        /// <summary>
        /// Binds a list of subsidies to the grid.
        /// </summary>
        /// <param name="subsidies"></param>
        void BindSubsidies(IList<ISubsidy> subsidies);

        #endregion


    }
}
