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
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// Interface for FixedDebitOrder 
    /// </summary>
    public interface IFixedDebitOrderSummary : IViewBase
    {
        #region Properties
        /// <summary>
        /// Set visibility of buttons
        /// </summary>
        bool ShowButtons { set;}
        /// <summary>
        /// Sets visibility of Updateable controls
        /// </summary>
        bool ShowUpdateableControl { set;}
        /// <summary>
        /// Show Interest Only Panel if Loan is Interest Only
        /// </summary>
        bool ShowInterestOnly { set;}
        /// <summary>
        /// Select first row on Future Grid
        /// </summary>
        bool selectedFirstRow { set;}
        
        #endregion

        #region Methods

        /// <summary>
        /// Bind FutureDated Grid
        /// </summary>
        /// <param name="futureDatedChangeLst"></param>
        void BindFutureDatedDOGrid(IList<IFutureDatedChange> futureDatedChangeLst);
        /// <summary>
        /// Bind Data on Interest only Panel - in the case of Interest Only
        /// </summary>
        /// <param name="totalAmortisingInstallment"></param>
        void BindInterestOnlyData(double totalAmortisingInstallment);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account">The mortgage loan account.</param>
        void BindFixedDebitOrderData(IAccount account);
        /// <summary>
        /// Bind Account Summary Grid
        /// </summary>
        /// <param name="accountLst"></param>
        void BindAccountSummaryGrid(IEventList<IAccount> accountLst);
        /// <summary>
        /// Set GridPostBack - single click in the case of Update and Delete
        /// </summary>
        void SetGridPostBack();
        /// <summary>
        /// Setup update controls on view
        /// </summary>
        void SetControlForUpdate();
        /// <summary>
        /// Setup controls for delete on view
        /// </summary>
        void SetControlForDelete();
        /// <summary>
        /// This method sets up the Initial data on the View prior to any record being selected
        /// on the Grid. When an item is selected on the Future Dated Changeds Grid, this data is then
        /// overwritten with the data as per the selected index
        /// </summary>
        /// <param name="account"></param>
        void SetUpInitialDataOnView(IAccount account);
        /// <summary>
        /// Bind Updateable controls
        /// </summary>
        /// <param name="futureDatedChange"></param>
        void BindUpdateableControlsData(IFutureDatedChange futureDatedChange);
        /// <summary>
        /// Get Updated FDC
        /// </summary>
        /// <param name="futureDatedChange"></param>
        /// <returns></returns>
        IFutureDatedChange GetUpdatedFDChange(IFutureDatedChange futureDatedChange);
        /// <summary>
        /// Get New FDC to insert
        /// </summary>
        /// <param name="futureDatedChange"></param>
        /// <returns></returns>
        IFutureDatedChange GetCapturedFutureDatedChange(IFutureDatedChange futureDatedChange);
        /// <summary>
        /// Get New FDC Detail to insert
        /// </summary>
        /// <param name="futureDatedChangeDetail"></param>
        /// <returns></returns>
        IFutureDatedChangeDetail GetCapturedFutureDatedChangeDetail(IFutureDatedChangeDetail futureDatedChangeDetail);
        /// <summary>
        /// Get effective date captured on screen
        /// </summary>
        DateTime? GetEffectiveDateCaptured { get; }

        /// <summary>
        /// Gets the updated FixedDebitOrderAmount value from the view
        /// </summary>
        double UpdatedFixedDebitOrderAmount { get; }

        /// <summary>
        /// Gets/sets the visibility of the submit button.
        /// </summary>
        bool SubmitButtonVisible { get; set; }

        #endregion


        #region eventhandlers
        /// <summary>
        /// Selected Index Change
        /// </summary>
        event KeyChangedEventHandler OnFutureOrderGridSelectedIndexChanged;
        /// <summary>
        /// Update button clicked
        /// </summary>
        event KeyChangedEventHandler UpdateButtonClicked;
        /// <summary>
        /// Delete button clicked
        /// </summary>
        event KeyChangedEventHandler DeleteButtonClicked;
        /// <summary>
        /// Cancel button clicked
        /// </summary>
        event KeyChangedEventHandler CancelButtonClicked;
        
        #endregion


    }
}
