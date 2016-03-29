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
using SAHL.Common.UI;

namespace SAHL.Web.Views.Common.Presenters
{    
    /// <summary>
    /// Presenter for Legal Entity Domicilium Address Update
    /// </summary>
    public class LegalEntityDomiciliumAddressUpdateWorkflow : LegalEntityDomiciliumAddressBase
    {
        List<AddressBindableObject> bindableAddresses;
        IApplicationRole applicationRole;

        /// <summary>
        /// Constructor for Domicilium Address Update
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityDomiciliumAddressUpdateWorkflow(ILegalEntityDomiciliumAddress view, SAHLCommonBaseController controller)
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

            // walk up tree till we get a node with application key
            CBONode parentNode = node.GetParentNodeByType(SAHL.Common.Globals.GenericKeyTypes.Offer);
            int applicationKey = parentNode.GenericKey;

            // get the application role for this legalentity
            applicationRole = base.legalEntity.GetApplicationRoleClient(applicationKey); 


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
                if (legalEntityAddress.Address.AddressFormat.Key == ((int)SAHL.Common.Globals.AddressFormats.Street) && legalEntityAddress.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
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

            // add property address only if it doesnt already exist in the collection as a legalentity address
            IApplicationMortgageLoan applicationMortgageLoan = applicationRepo.GetApplicationByKey(applicationKey) as IApplicationMortgageLoan;

            if (applicationMortgageLoan != null && applicationMortgageLoan.Property != null)
            {
                if (!base.CheckAddressInCollection(applicationMortgageLoan.Property.Address, bindableAddresses))
                {
                    bindableAddresses.Add(new AddressBindableObject(applicationMortgageLoan.Property.Address, (int)SAHL.Common.Globals.DomiciliumAddressTypes.Property));
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
                    ILegalEntityDomicilium legalEntityDomicilium = null;

                    // if the guy selects a property address and it is NOT a legalentity address create one.
                    ILegalEntityAddress legalEntityAddress = legalEntity.LegalEntityAddresses.Where(x => x.Address.Key == _view.SelectedAddressKey
                        && x.GeneralStatus.Key == (int)GeneralStatuses.Active).FirstOrDefault();
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

                    // check if there is already a pending LegalEntityDomicilium record - if there is then update it otherwise create a new pending LegalEntityDomicilium
                    if (applicationRole.ApplicationRoleDomicilium != null
                        && applicationRole.ApplicationRoleDomicilium.LegalEntityDomicilium != null
                        && applicationRole.ApplicationRoleDomicilium.LegalEntityDomicilium.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Pending)
                    {
                        legalEntityDomicilium = applicationRole.ApplicationRoleDomicilium.LegalEntityDomicilium;
                    }
                    else
                        legalEntityDomicilium = legalentityRepo.CreateEmptyLegalEntityDomicilium();

                    legalEntityDomicilium.ADUser = osRepo.GetAdUserForAdUserName(_view.CurrentPrincipal.Identity.Name);
                    legalEntityDomicilium.GeneralStatus = lookupRepo.GetGeneralStatuses()[GeneralStatuses.Pending];
                    legalEntityDomicilium.LegalEntityAddress = legalEntityAddress;
                    legalEntityDomicilium.ChangeDate = DateTime.Now;

                    // link our new LegalEntityDomicilium to the new ILegalEntityAddress record 
                    legalEntityAddress.LegalEntityDomiciliums.Add(_view.Messages, legalEntityDomicilium);
                    legalentityRepo.SaveLegalEntityAddress(legalEntityAddress, legalEntityAddress.Address);

                    IApplicationRoleDomicilium applicationRoleDomicilium = null;

                    // if an ApplicationRoleDomicilium already exists we will update it, otherwise create a new one
                    if (applicationRole.ApplicationRoleDomicilium != null)
                        applicationRoleDomicilium = applicationRole.ApplicationRoleDomicilium;
                    else
                        applicationRoleDomicilium = applicationRepo.CreateEmptyApplicationRoleDomicilium();

                    applicationRoleDomicilium.LegalEntityDomicilium = legalEntityDomicilium;

                    applicationRoleDomicilium.ADUser = osRepo.GetAdUserForAdUserName(_view.CurrentPrincipal.Identity.Name);
                    applicationRoleDomicilium.ApplicationRole = applicationRole;
                    applicationRoleDomicilium.ChangeDate = DateTime.Now;

                    applicationRole.ApplicationRoleDomicilium = applicationRoleDomicilium;

                    //save the application - this will save everything
                    applicationRepo.SaveApplication(applicationRole.Application);

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

            if (base.legalEntity.ActiveDomicilium != null && base.legalEntity.ActiveDomicilium.LegalEntityAddress.Address.Key != _view.SelectedAddressKey)
            {
                errorMessage = "Selected Pending Domicilium is not the same as the Active Domicilum Address.";
                _view.Messages.Add(new Warning(errorMessage, errorMessage));
            }
        }
    }
}
