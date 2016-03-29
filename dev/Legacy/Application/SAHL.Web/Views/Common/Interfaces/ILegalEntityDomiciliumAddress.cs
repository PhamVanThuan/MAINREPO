using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// Simple class for binding addresses to the grid.
    /// </summary>
    public class AddressBindableObject
    {
        public AddressBindableObject(IAddress address, int domiciliumAddressTypeKey, int accountKey = 0)
        {
            this.Address = address;
            this.DomiciliumAddressTypeKey = domiciliumAddressTypeKey;
            this.AccountKey = accountKey;
        }

        public int AddressKey
        {
            get { return Address.Key; }
        }

        public IAddress Address
        {
            get;
            set;
        }

        public int DomiciliumAddressTypeKey
        {
            get;
            set;
        }

        public int AccountKey
        {
            get;
            set;
        }

        public string FormattedAddress
        {
            get
            {
                if (AccountKey > 0)
                    return Address.GetFormattedDescription(AddressDelimiters.Comma) + " (Loan " + AccountKey + ")";
                else
                    return Address.GetFormattedDescription(AddressDelimiters.Comma);
            }
        }
    }

    /// <summary>
    /// Legal Entity Domicilium Address Interface
    /// </summary>
    public interface ILegalEntityDomiciliumAddress : IViewBase
    {
        #region eventhandlers

        /// <summary>
        /// Event Handler for Cancel Button Clicked
        /// </summary>
        event EventHandler onCancelButtonClicked;

        /// <summary>
        /// Event Handler for Submit Button Clicked
        /// </summary>
        event EventHandler onSubmitButtonClicked;

        #endregion eventhandlers

        #region Methods

        /// <summary>
        /// Set up view controls for display
        /// </summary>
        void SetControlsForDisplay(string groupingText);

        /// <summary>
        /// Set up view controls for Update
        /// </summary>
        void SetControlsForUpdate();

        /// <summary>
        /// Bind Domicilium Address text
        /// </summary>
        /// <param name="address"></param>
        void BindDomiciliumAddress(string address);

        /// <summary>
        /// Populate Address Grid
        /// </summary>
        /// <param name="addressList"></param>
        void PopulateAddressGrid(List<AddressBindableObject> addressList);

        #endregion Methods

        /// <summary>
        ///
        /// </summary>
        int SelectedAddressKey { get; set; }

        #region #21429 personal loans domicilium

        /// <summary>
        /// Used for personal loans domicilium address to show the active legal entity domicilium address, if one exists
        /// </summary>
        bool ShowActiveDomiciliumAddressRow { set; }

        /// <summary>
        /// Bind Active Domicilium address text
        /// </summary>
        /// <param name="address"></param>
        void BindActiveDomiciliumAddress(string address);

        #endregion #21429 personal loans domicilium
    }
}