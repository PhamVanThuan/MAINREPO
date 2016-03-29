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
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Collections.Generic;

namespace SAHL.Web.Views.Administration.Interfaces
{
    /// <summary>
    /// Subsidy Provider Interface
    /// </summary>
    public interface ISubsidyProvider : IViewBase
    {
        #region properties
        /// <summary>
        /// Set controls for Display - hides updateable controls
        /// </summary>
        bool SetControlsForDisplay { set;}
        /// <summary>
        /// Sets controls for update - hides display controls
        /// </summary>
        bool SetControlsForUpdate { set; }
        /// <summary>
        /// Set button visibility
        /// </summary>
        bool SetButtonVisibility { set; }
        /// <summary>
        /// Set cancel button visibility
        /// </summary>
        bool CancelButtonVisible { set;}
        /// <summary>
        /// Set submit button visibility
        /// </summary>
        bool SubmitButtonVisible { set;}
        /// <summary>
        /// Enable or Disable the update
        /// </summary>
        bool EnableDisableUpdate { set; }
        /// <summary>
        /// Gets the effective date to save to the DB.
        /// </summary>
        DateTime EffectiveDate { get; set;}
        /// <summary>
        /// If no Address Format selected, this control should not be visible.
        /// </summary>
        bool AddressDetailsVisible { set;}
        /// <summary>
        /// Set address panel to visible or not 
        /// </summary>
        bool pnlAddressVisible { set;}
        /// <summary>
        /// Set if we need to determine
        /// </summary>
        bool SetRebindAddress { set;}
        /// <summary>
        /// Set the default address type
        /// </summary>
        int SetAddressType { set;}

        #endregion

        #region Methods
        /// <summary>
        /// Bind subsidy Provider details based on provider chosen on ajax
        /// </summary>
        /// <param name="subsidyProvider"></param>
        void BindSubsidyProviderDetail(SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider subsidyProvider);
        /// <summary>
        /// Bind Drop Downs
        /// </summary>
        /// <param name="lookups"></param>
        void BindDropDowns(ILookupRepository lookups);
        /// <summary>
        /// Bind Subsidy Provider Address
        /// </summary>
        /// <param name="address"></param>
        void BindSubsidyProviderAddress (IAddress address);
        /// <summary>
        /// Get Captured Subsidy Provider
        /// </summary>
        /// <param name="subsidyProvider"></param>
        /// <returns></returns>
        SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider GetCapturedSubsidyProvider(SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider subsidyProvider);
        /// <summary>
        /// Get updated Subsidy provider address
        /// </summary>
        /// <param name="subsidyProviderAddress"></param>
        /// <returns></returns>
        IAddress GetCapturedSubsidyProviderAddress(IAddress subsidyProviderAddress);
        /// <summary>
        /// get captured subsidy provider address
        /// </summary>
        IAddress GetCapturedAddress { get; }

        /// <summary>
        /// Gets the key of the address type that is selected on the view.
        /// </summary>
        string SelectedAddressTypeValue { get; }

        /// <summary>
        /// Get captured susbidy provider object for Add
        /// </summary>
        /// <param name="subsidyProviderNew"></param>
        /// <returns></returns>
        SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider GetSusbidyProviderForAdd(SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider subsidyProviderNew);
        /// <summary>
        /// disable subsidy type update
        /// </summary>
        void DisableSusbidyTypeUpdate();
        /// <summary>
        /// Gets the key of the address format that is selected on the view.
        /// </summary>
        string SelectedAddressFormatValue { get; }

        /// <summary>
        /// Gets/sets whether the ajax call to retrieve subsidy providers is enabled.
        /// </summary>
        bool SubsidyProviderAjaxEnabled { get; set; }


        #endregion

        #region EventHandlers
        /// <summary>
        /// Rebind subsidy details
        /// </summary>
        event KeyChangedEventHandler OnReBindSubsidyDetails;
        /// <summary>
        /// submit button clicked
        /// </summary>
        event EventHandler OnSubmitButtonClicked;
        /// <summary>
        /// cancel button clicked
        /// </summary>
        event EventHandler OnCancelButtonClicked;
        /// <summary>
        /// Add Address button clicked - this is a a hidden functionality that is called when the user
        /// selects an address on the address ajax control
        /// </summary>
        event KeyChangedEventHandler AddAddressButtonClicked;

        #endregion


    }

}
