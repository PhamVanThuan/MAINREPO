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
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Controls;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common.Interfaces
{

    /// <summary>
    /// Provides a list of available buttons for the view.
    /// </summary>
    public enum AddressViewButtons
    {
        Add,
        Update,
        Delete
    }

    /// <summary>
    /// Interface for an address view.
    /// </summary>
    public interface IAddressView : IViewBase
    {


        #region Events

        /// <summary>
        /// Event raised when an existing address is selected.
        /// </summary>
        event KeyChangedEventHandler ExistingAddressSelected;

        /// <summary>
        /// Event raised when the selected address type is changed.
        /// </summary>
        event KeyChangedEventHandler SelectedAddressTypeChanged;

        /// <summary>
        /// Raised when the add button is clicked.
        /// </summary>
        event EventHandler AddButtonClicked;

        /// <summary>
        /// Raised when the audit button is clicked.
        /// </summary>
        event EventHandler AuditButtonClicked;

        /// <summary>
        /// Raised when the cancel button is clicked.
        /// </summary>
        event EventHandler CancelButtonClicked;

        /// <summary>
        /// Raised when the delete button is clicked.
        /// </summary>
        event EventHandler DeleteButtonClicked;

        /// <summary>
        /// Raised when the update button is clicked.
        /// </summary>
        event EventHandler UpdateButtonClicked;

        /// <summary>
        /// Used by the application wizard and raised when the back button is clicked
        /// </summary>
        event EventHandler BackButtonClicked;

        /// <summary>
        /// Raised when the selected index of the address grid is changed
        /// </summary>
        event EventHandler GridAddressSelectedIndexChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Sets whether the read-only address details are visible.
        /// </summary>
        bool AddressDetailsVisible { set; }

        /// <summary>
        /// Sets whether the address format field is read only.
        /// </summary>
        bool AddressFormatReadOnly { set; }


        /// <summary>
        /// Sets whether to display the legalEntity column on the address grid.
        /// </summary>
        bool ShowLegalEntityNameOnAddressGrid { set;}


        /// <summary>
        /// Sets whether the address input form is visible.
        /// </summary>
        bool AddressFormVisible { set; }

        /// <summary>
        /// Sets the header text of the address list area.
        /// </summary>
        string AddressListHeaderText { set; }

        /// <summary>
        /// Sets whether the list of addresses is visible.
        /// </summary>
        bool AddressListVisible { set; }

        /// <summary>
        /// Sets whether the back button is visible.
        /// </summary>
        bool BackButtonVisible { set;}

        /// <summary>
        /// Gets the address repository object used by the view to retrieve address information.
        /// </summary>
        IAddressRepository AddressRepository { get; }

        /// <summary>
        /// Sets whether the address status fields should be visible or not.
        /// </summary>
        bool AddressStatusVisible { set; }

        /// <summary>
        /// Sets whether the legal entity specific fields should be visible or not.
        /// </summary>
        bool LegalEntityInfoVisible { set; }

        /// <summary>
        /// Sets whether the address type field is read only.
        /// </summary>
        bool AddressTypeReadOnly { set; }

        AddressTypes AddressType { set;}

        /// <summary>
        /// Sets whether the add button is visible.
        /// </summary>
        bool AddButtonVisible { set; }

        /// <summary>
        /// Sets the Add button text.
        /// </summary>
        string AddButtonText { set;}

        /// <summary>
        /// Sets whether the Audit Trail button is visible.
        /// </summary>
        bool AuditTrailButtonVisible { set; }

        /// <summary>
        /// Sets whether the Cancel button is visible.
        /// </summary>
        bool CancelButtonVisible { set; }

        /// <summary>
        /// Sets whether the Delete button is visible.
        /// </summary>
        bool DeleteButtonVisible { set; }

        /// <summary>
        /// Sets whether the Update button is visible.
        /// </summary>
        bool UpdateButtonVisible { set; }


        /// <summary>
        /// Sets whether the add button is enabled.
        /// </summary>
        bool AddButtonEnabled { set;}


        /// <summary>
        /// Sets the text on the update button
        /// </summary>
        string UpdateButtonText { set;}

        /// <summary>
        /// Sets the Legal Entity property for the view. Used by the Application Wizard.
        /// </summary>
        ILegalEntity LegalEntity { set;}

        /// <summary>
        /// Sets whether the "Type" column on the grid is visible.
        /// </summary>
        bool GridColumnTypeVisible { set; }

        /// <summary>
        /// Sets whether the "Effective Date" column on the grid is visible.
        /// </summary>
        bool GridColumnEffectiveDateVisible { set; }

        /// <summary>
        /// Sets whether the "Status" column on the grid is visible.
        /// </summary>
        bool GridColumnStatusVisible { set; }

        /// <summary>
        /// Gets/sets whether the address grid causes a postback.  
        /// </summary>
        /// <remarks>
        /// This must be set BEFORE data is bound to the grid, because the grid rendering is poorly implemented and 
        /// occurs in the RowDataBound override.
        /// </remarks>
        bool GridPostBack { get; set; }

        /// <summary>
        /// Gets/sets the selected index of the grid.
        /// </summary>
        int GridSelectedIndex { get; set; }

        /// <summary>
        /// Sets the format of the address.
        /// </summary>
        AddressFormats AddressFormat { get; set;}

        /// <summary>
        /// Gets the address that is currently selected on the grid.  This will return a different type of 
        /// object depending on the <see cref="SelectedAddressSource"/>
        /// </summary>
        object SelectedAddress { get; }

        /// <summary>
        /// Gets the source of the currently selected address.
        /// </summary>
        AddressSources SelectedAddressSource { get; }

        /// <summary>
        /// Gets the key of the address type that is selected on the view.
        /// </summary>
        string SelectedAddressTypeValue { get; }

        /// <summary>
        /// Gets/sets whether to also display inactive addresses on the grid.
        /// </summary>
        bool ShowInactive { get; set; }

        /// <summary>
        /// Gets the effective date to save to the DB.
        /// </summary>
        DateTime? EffectiveDate { get; }

        /// <summary>
        /// Gets the status of the address (Active/Inactive)
        /// </summary>
        GeneralStatuses AddressStatus { get; }

        /// <summary>
        /// Gets/sets whether the control should automatically populate the input fields with 
        /// the details of the address selected in the grid.
        /// </summary>
        bool PopulateInputFromGrid { get; set; }

        /// <summary>
        /// Gets/sets the title text displayed on the control.  If this is a set, a title will 
        /// display between the grid and the input form.
        /// </summary>
        string TitleText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool ButtonRowVisible { set;}

        #endregion

        #region Methods

        /// <summary>
        /// Binds a list of addresses to the address list grid.
        /// </summary>
        /// <param name="addresses"></param>
        void BindAddressList(IEventList<IAddress> addresses);

        /// <summary>
        /// Binds a list of legal entity addresses to the address list grid.
        /// </summary>
        /// <param name="addresses"></param>
        void BindAddressList(IEventList<ILegalEntityAddress> addresses);

        /// <summary>
        /// Binds a list of legal entity addresses and failed migration addresses to the address list grid.
        /// </summary>
        /// <param name="addresses"></param>
        /// <param name="failedAddresses"></param>
        void BindAddressList(IEventList<ILegalEntityAddress> addresses, IEventList<IFailedLegalEntityAddress> failedAddresses);

        /// <summary>
        /// Binds a dictionary of AddressFormat lookups to a drop down box.
        /// </summary>
        /// <param name="addressFormats"></param>
        void BindAddressFormats(IDictionary<int, string> addressFormats);

        /// <summary>
        /// Binds a collection of <see cref="IGeneralStatus"/> objects to a drop down box.
        /// </summary>
        /// <param name="addressStatuses"></param>
        void BindAddressStatuses(ICollection<IGeneralStatus> addressStatuses);

        /// <summary>
        /// Binds a dictionary of <see cref="IAddressType"/> lookups to a drop down box.
        /// </summary>
        /// <param name="addressTypes"></param>
        void BindAddressTypes(IDictionary<int, string> addressTypes);

        /// <summary>
        /// Returns a new address entity populated with the values entered into the address form.
        /// </summary>
        /// <returns></returns>
        IAddress GetCapturedAddress();

        /// <summary>
        /// Returns <c>address</c> populated with the values entered into the address form.
        /// </summary>
        /// <returns></returns>
        IAddress GetCapturedAddress(IAddress address);

        /// <summary>
        /// Sets the address populated in the capture form.
        /// </summary>
        /// <param name="address"></param>
        void SetAddress(IAddress address);

        /// <summary>
        /// Sets the address populated in the capture form.
        /// </summary>
        /// <param name="address"></param>
        void SetAddress(ILegalEntityAddress address);

        #endregion
    }
}
