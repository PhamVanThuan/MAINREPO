using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Collections.Generic;
using SAHL.Common.Exceptions;
using SAHL.Common.DomainMessages;
using System.Linq;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Common.Presenters
{    
    /// <summary>
    /// Presenter for Legal Entity Domicilium Address Update
    /// </summary>
    public class LegalEntityDomiciliumAddressUpdate : LegalEntityDomiciliumAddressBase
    {
        List<AddressBindableObject> bindableAddresses;

        /// <summary>
        /// Constructor for Domicilium Address Update
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityDomiciliumAddressUpdate(ILegalEntityDomiciliumAddress view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }
        /// <summary>
        /// OnViewInitialised event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            
            if (!_view.ShouldRunPage) 
                return;

            _view.onSubmitButtonClicked += (_view_onSubmitButtonClicked);
            _view.onCancelButtonClicked += (_view_onCancelButtonClicked);

            // add the active legal entity domicilium address if there is one
            bindableAddresses = new List<AddressBindableObject>();

            if (base.legalEntity != null && base.legalEntity.ActiveDomicilium != null)
            {
                bindableAddresses.Add(new AddressBindableObject(base.legalEntity.ActiveDomicilium.LegalEntityAddress.Address, (int)SAHL.Common.Globals.DomiciliumAddressTypes.Active));
            }

            // add legal entity addresses (except if it is already the legal entity domicilium address)
            foreach (var legalEntityAddress in base.legalEntity.LegalEntityAddresses)
            {
                if (legalEntityAddress.Address.AddressFormat.Key == ((int)SAHL.Common.Globals.AddressFormats.Street) && legalEntityAddress.GeneralStatus.Key==(int)SAHL.Common.Globals.GeneralStatuses.Active)
                {
                    if (legalEntityAddress.Address.Key != (base.legalEntity.ActiveDomicilium != null ? base.legalEntity.ActiveDomicilium.LegalEntityAddress.Address.Key : -1))
                    {
                        if (!base.CheckAddressInCollection(legalEntityAddress.Address, bindableAddresses))
                        {
                            bindableAddresses.Add(new AddressBindableObject(legalEntityAddress.Address, (int)SAHL.Common.Globals.DomiciliumAddressTypes.LegalEntity));
                        }
                    }
                }
            }

            // add property addresses only if they dont already exist in the collection as a legalentity address
            // we must get all property addresses for this legal entity
            foreach (var role in legalEntity.Roles)
            {
                IMortgageLoanAccount mla = role.Account as IMortgageLoanAccount;
                if (mla != null)
                {
                    if (mla.SecuredMortgageLoan != null && !base.CheckAddressInCollection(mla.SecuredMortgageLoan.Property.Address, bindableAddresses))
                        bindableAddresses.Add(new AddressBindableObject(mla.SecuredMortgageLoan.Property.Address, (int)SAHL.Common.Globals.DomiciliumAddressTypes.Property, role.Account.Key)); // pass in the loan number here
                }
            } 

            _view.SetControlsForUpdate();
            _view.PopulateAddressGrid(bindableAddresses);

        }

        void _view_onSubmitButtonClicked(object sender, EventArgs e)
        {
            ValidateScreenInput();

            if (_view.IsValid == false)
                return;

            IAddress selectedAddress = addressRepo.GetAddressByKey(_view.SelectedAddressKey);
            if (selectedAddress != null)
            {
                TransactionScope txn = new TransactionScope();

                try
                {
                    // if the guy selects a property address and it is NOT a legalentity address create one.
                    ILegalEntityAddress legalEntityAddress = legalEntity.LegalEntityAddresses.Where(x => x.Address.Key == _view.SelectedAddressKey 
                        && x.GeneralStatus.Key==(int)GeneralStatuses.Active).FirstOrDefault();
                    if (legalEntityAddress == null)
                    {
                        legalEntityAddress = base.addressRepo.CreateEmptyLegalEntityAddress();
                        legalEntityAddress.Address = selectedAddress;
                        legalEntityAddress.AddressType = addressRepo.GetAddressTypeByKey((int)AddressTypes.Residential);
                        legalEntityAddress.EffectiveDate = DateTime.Now;
                        legalEntityAddress.GeneralStatus = lookupRepo.GeneralStatuses[GeneralStatuses.Active];
                        legalEntityAddress.LegalEntity = legalEntity;
                        addressRepo.SaveLegalEntityAddress(legalEntityAddress);
                    }

                    // create a new LegalEntityDomicilium
					ILegalEntityDomicilium legalEntityDomicilium = legalentityRepo.CreateEmptyLegalEntityDomicilium();
                    legalEntityDomicilium.LegalEntityAddress = legalEntityAddress;
					legalEntityDomicilium.ADUser = osRepo.GetAdUserForAdUserName(_view.CurrentPrincipal.Identity.Name);
					legalEntityDomicilium.GeneralStatus = lookupRepo.GeneralStatuses[GeneralStatuses.Active];
					legalEntityDomicilium.ChangeDate = DateTime.Now;

                    legalEntityAddress.LegalEntityDomiciliums.Add(_view.Messages, legalEntityDomicilium);

                    // set this to true - the property contains the logic to set all existing legalEntityDomiciliums to inactive
                    legalEntityAddress.IsActiveDomicilium = true;
                    

                    //save the legal entity - this will save everything
                    legalentityRepo.SaveLegalEntity(legalEntity, false);

                    txn.VoteCommit();

                     _view.Navigator.Navigate("Submit");
                }
                catch (Exception)
                {
                    txn.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
                finally
                {
                    txn.Dispose();
                }
            }             
        }

        void _view_onCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        private void ValidateScreenInput()
        {
            string errorMessage = "";

            if (_view.SelectedAddressKey <= 0)
            {
                errorMessage = "Must select an Address.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            if (base.legalEntity.ActiveDomicilium != null && base.legalEntity.ActiveDomicilium.LegalEntityAddress.Address.Key == _view.SelectedAddressKey)
            {
                errorMessage = "The selected Address is already the Domicilium Address.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }
        }

    }
}
