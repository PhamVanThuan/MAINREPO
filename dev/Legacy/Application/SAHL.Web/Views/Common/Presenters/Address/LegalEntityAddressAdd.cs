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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Controls;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common.Presenters.Address
{
    /// <summary>
    /// Presenter used to display an address for a legal entity.
    /// </summary>
    public class LegalEntityAddressAdd : LegalEntityAddressBase
    {

        public LegalEntityAddressAdd(IAddressView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            // make sure the grid doesn't allow post back - this needs to happen BEFORE the bind event because of the way the grid is 
            // implemented
            _view.GridPostBack = false;

            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            // set up event handlers
            _view.SelectedAddressTypeChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_SelectedAddressTypeChanged);
            _view.ExistingAddressSelected += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_ExistingAddressSelected);

            // bind lookup data
            _view.BindAddressTypes(LookupRepository.AddressTypes);
            _view.BindAddressFormats(LookupRepository.AddressFormatsByAddressType((AddressTypes)Int32.Parse(_view.SelectedAddressTypeValue)));

            // button event handlers
            _view.AddButtonClicked += new EventHandler(_view_AddButtonClicked);
            _view.CancelButtonClicked += new EventHandler(_view_CancelButtonClicked);


        }

        void _view_SelectedAddressTypeChanged(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            _view.BindAddressFormats(LookupRepository.AddressFormatsByAddressType((AddressTypes)Convert.ToInt32(e.Key)));
        }

        void _view_AddButtonClicked(object sender, EventArgs e)
        {
            IAddress address = _view.GetCapturedAddress();

            SaveAndNavigate(address, "Add");
        }

        void _view_ExistingAddressSelected(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            IAddress address = AddressRepository.GetAddressByKey(Convert.ToInt32(e.Key));
            if (address.ValidateEntity())
            {
                if (!SaveAndNavigate(address, "Add"))
                    _view.SetAddress(address);
            }
            else
                _view.SetAddress(address);

        }

        void _view_CancelButtonClicked(object sender, EventArgs e)
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
            {
                string navigateTo = GlobalCacheData[ViewConstants.NavigateTo].ToString();
                ClearCachedData();
                _view.Navigator.Navigate(navigateTo);
            }
            else
                _view.Navigator.Navigate("Cancel");
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            _view.GridPostBack = false;

            base.OnViewPreRender(sender, e);

            // set panel display properties
            _view.AddressListHeaderText = "Legal Entity Addresses";
            _view.AddressListVisible = true;
            _view.AddressFormVisible = true;

            // make buttons visible
            _view.CancelButtonVisible = true;
            _view.AddButtonVisible = true;
        }

        private bool SaveAndNavigate(IAddress address, string navigateValue)
        {
            // make sure a valid effective date has been entered - if not then exit here (this is 
            // done at this level not at the domain level as this is specific to this set of 
            // presenters - some presenters using the address view just use a default value
            if (!_view.EffectiveDate.HasValue)
            {
                _view.Messages.Add(new Error("Effective date is a mandatory field", ""));
                address.ValidateEntity();
                return false;
            }

            TransactionScope txn = new TransactionScope();
            try
            {
                IAddressType addressType = AddressRepository.GetAddressTypeByKey(Int32.Parse(_view.SelectedAddressTypeValue));
                LegalEntityRepository.SaveAddress(addressType, LegalEntity, address, _view.EffectiveDate.Value);

                txn.VoteCommit();
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.Messages.Count > 0)
                    return false;
                throw;
            }
            finally
            {
                txn.Dispose();
            }
            if (_view.IsValid)
            {
                if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
                {
                    string navigateTo = GlobalCacheData[ViewConstants.NavigateTo].ToString();
                    ClearCachedData();
                    _view.Navigator.Navigate(navigateTo);
                }
                else
                    _view.Navigator.Navigate(navigateValue);

                return true;
            }
            return false;
        }


    }
}
