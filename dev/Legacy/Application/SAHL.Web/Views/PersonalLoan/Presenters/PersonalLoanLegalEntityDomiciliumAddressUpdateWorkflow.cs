using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Web.Views.Common.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    /// <summary>
    /// Presenter for Legal Entity Domicilium Address Update
    /// </summary>
    public class PersonalLoanLegalEntityDomiciliumAddressUpdateWorkflow : LegalEntityDomiciliumAddressBase
    {
        private List<AddressBindableObject> bindableAddresses;
        private IExternalRole externalRole;

        /// <summary>
        /// Constructor for Domicilium Address Update
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PersonalLoanLegalEntityDomiciliumAddressUpdateWorkflow(ILegalEntityDomiciliumAddress view, SAHLCommonBaseController controller)
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

            // get the external role for this legalentity
            externalRole = base.legalEntity.GetActiveClientExternalRoleForOffer(parentNode.GenericKey);

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

            _view.SetControlsForUpdate();
            _view.PopulateAddressGrid(bindableAddresses);
        }

        private void _view_onSubmitButtonClicked(object sender, EventArgs e)
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

                    IExternalRoleDomicilium pendingExternalRoleDomicilium = externalRole.PendingExternalRoleDomicilium;

                    // check if there is already a pending LegalEntityDomicilium record - if there is then update it otherwise create a new pending LegalEntityDomicilium
                    if (pendingExternalRoleDomicilium != null)
                    {
                        legalEntityDomicilium = pendingExternalRoleDomicilium.LegalEntityDomicilium;
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

                    IExternalRoleDomicilium externalRoleDomicilium = null;

                    // if an ExternalRoleDomicilium already exists we will update it, otherwise create a new one
                    if (pendingExternalRoleDomicilium != null)
                        externalRoleDomicilium = pendingExternalRoleDomicilium;
                    else
                        externalRoleDomicilium = applicationRepo.CreateEmptyExternalRoleDomicilium();

                    externalRoleDomicilium.LegalEntityDomicilium = legalEntityDomicilium;

                    externalRoleDomicilium.ADUser = osRepo.GetAdUserForAdUserName(_view.CurrentPrincipal.Identity.Name);
                    externalRoleDomicilium.ExternalRole = externalRole;
                    externalRoleDomicilium.ChangeDate = DateTime.Now;

                    externalRole.PendingExternalRoleDomicilium = externalRoleDomicilium;

                    legalentityRepo.SaveExternalRole(externalRole);

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

        private void _view_onCancelButtonClicked(object sender, EventArgs e)
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