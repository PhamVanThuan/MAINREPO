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
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.Address
{
    /// <summary>
    /// Presenter used to display an address for a legal entity.
    /// </summary>
    public class LegalEntityAddressUpdate : LegalEntityAddressBase
    {

        public LegalEntityAddressUpdate(IAddressView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            // bind lookup data
            _view.BindAddressStatuses(LookupRepository.GeneralStatuses.Values);
            _view.BindAddressTypes(LookupRepository.AddressTypes);

            // button event handlers
            _view.CancelButtonClicked += new EventHandler(_view_CancelButtonClicked);
            _view.UpdateButtonClicked += new EventHandler(_view_UpdateButtonClicked);
            _view.ExistingAddressSelected += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_ExistingAddressSelected);

            // if it's the first load, set the format according to the list supplied
            if (!_view.IsPostBack && LegalEntity.LegalEntityAddresses.Count > 0)
            {
                _view.AddressFormat = (AddressFormats)LegalEntity.LegalEntityAddresses[0].Address.AddressFormat.Key;
            }

        }

        void _view_ExistingAddressSelected(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            IAddress address = AddressRepository.GetAddressByKey(Convert.ToInt32(e.Key));
            _view.SetAddress(address);

            if (address.ValidateEntity())
            {
                SaveAndNavigate(address);
            }
        }

        void _view_UpdateButtonClicked(object sender, EventArgs e)
        {
            IAddress address = _view.GetCapturedAddress();
            SaveAndNavigate(address);
        }

        void _view_CancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return; 

            // set panel display properties
            _view.AddressListHeaderText = "Legal Entity Addresses";
            _view.AddressListVisible = true;
            _view.AddressFormVisible = true;
            _view.AddressStatusVisible = true;
            _view.AddressFormatReadOnly = true;
            _view.AddressTypeReadOnly = true;

            // bind address formats - also needs to be done late as addresses change when the grid is selected
            _view.BindAddressFormats(LookupRepository.AddressFormatsByAddressType((AddressTypes)Int32.Parse(_view.SelectedAddressTypeValue)));

            // make buttons visible
            _view.CancelButtonVisible = true;
            _view.UpdateButtonVisible = (_view.SelectedAddress != null);

            // if the selected address is a failed one, we need to manually set the address type
            if (_view.SelectedAddressSource == AddressSources.FailedLegalEntityAddress)
            {
                IFailedLegalEntityAddress failedAddress = _view.SelectedAddress as IFailedLegalEntityAddress;
                if (failedAddress.FailedStreetMigration == null)
                    _view.AddressType = AddressTypes.Postal;
                else
                    _view.AddressType = AddressTypes.Residential;
            }
        }

        private bool SaveAndNavigate(IAddress address)
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
                // for legal entity addresses, just update them
                ILegalEntityAddress leAddress = _view.SelectedAddress as ILegalEntityAddress;
                IRuleService ruleService = ServiceFactory.GetService<IRuleService>();
                ruleService.ExecuteRule(_view.Messages, "LegalEntityAddressChangesWillUpdateActiveDomicilium", leAddress);
                ruleService.ExecuteRule(_view.Messages, "LegalEntityAddressChangesWillUpdatePendingDomicilium", leAddress);
                //WhenChangingPropertyAddressDetailsWarnUserOfLegalEntitiesUsingAsDomicilium
                
                if (leAddress != null)
                {
                    leAddress.EffectiveDate = _view.EffectiveDate.Value;
                    leAddress.GeneralStatus = LookupRepository.GeneralStatuses[_view.AddressStatus];
                    LegalEntityRepository.SaveLegalEntityAddress(leAddress, address);
                }
                else
                {
                    // in the case of dirty addresses, we're actually just saving a new one and marking the 
                    // old one as cleaned
                    IFailedLegalEntityAddress dirtyAddress = View.SelectedAddress as IFailedLegalEntityAddress;
                    IAddressType addressType = AddressRepository.GetAddressTypeByKey(Int32.Parse(_view.SelectedAddressTypeValue));
                    LegalEntityRepository.SaveAddress(addressType, LegalEntity, address, _view.EffectiveDate.Value);
                    AddressRepository.CleanDirtyAddress(dirtyAddress);
                }

                txn.VoteCommit();

                _view.Navigator.Navigate("Update");
                return true;

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
        }

    }
}
