using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI;
using Castle.ActiveRecord;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using SAHL.Common.Exceptions;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.Administration.Presenters
{   
    /// <summary>
    /// Subsidy Provider Edit
    /// </summary>
    public class SubsidyProviderEdit : SubsidyProviderBase
    {
        private bool _rebindAddress; // = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public SubsidyProviderEdit(SAHL.Web.Views.Administration.Interfaces.ISubsidyProvider view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }
        /// <summary>
        /// OnViewInitialised
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!View.ShouldRunPage) return;

            _view.SetButtonVisibility = true;
            _view.SetControlsForUpdate = false;

            _view.BindDropDowns(lookups);

            _view.DisableSusbidyTypeUpdate();

            _view.OnReBindSubsidyDetails += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnReBindSubsidyDetails);
            _view.AddAddressButtonClicked+=new KeyChangedEventHandler(_view_AddAddressButtonClicked);
            _view.OnSubmitButtonClicked+=new EventHandler(_view_OnSubmitButtonClicked);
        }
        /// <summary>
        /// OnViewPreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            if (PrivateCacheData.ContainsKey(SelectedSubsidyProvider) && PrivateCacheData[SelectedSubsidyProvider] != null)
            {
                subsidyProvider = PrivateCacheData[SelectedSubsidyProvider] as SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider;
                subsidyProvider = empRepo.GetSubsidyProviderByKey(subsidyProvider.Key);
            }

            //Set Default Address Type To Postal as it is the only Address Type in the drop down list
            _view.SetAddressType = (int)AddressTypes.Postal;
            _view.SetRebindAddress = _rebindAddress;

            if (subsidyProvider != null)
                    _view.BindSubsidyProviderDetail(subsidyProvider);
            else
                _view.pnlAddressVisible = false;

            // Reset rebind address
            if (_rebindAddress)
                _rebindAddress = false;

            if (PrivateCacheData.ContainsKey(SubsidyProviderAddress) && PrivateCacheData[SubsidyProviderAddress] != null)
            {
                address = PrivateCacheData[SubsidyProviderAddress] as SAHL.Common.BusinessModel.Interfaces.IAddress;
                address = addressRepository.GetAddressByKey(address.Key);
            }
            else
                address = null;

            if (_view.IsPostBack && address != null)
                _view.BindSubsidyProviderAddress(address);

            if ((_view.IsPostBack && address == null) &&
                (_view.SelectedAddressTypeValue == "-select-" || _view.SelectedAddressFormatValue == "-select-"))
                {
                    _view.AddressDetailsVisible = false;
                }
        }

        void _view_AddAddressButtonClicked(object sender, KeyChangedEventArgs e)
        {
            IAddress addressSelected = addressRepository.GetAddressByKey(Convert.ToInt32(e.Key));

            if (addressSelected != null)
            {
                if (PrivateCacheData.ContainsKey(SubsidyProviderAddress))
                    PrivateCacheData[SubsidyProviderAddress] = addressSelected;
                else
                    PrivateCacheData.Add(SubsidyProviderAddress, addressSelected);
            }
        }
        /// <summary>
        /// Rebind subsidy details based on provider selected on ajax control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _view_OnReBindSubsidyDetails(object sender, KeyChangedEventArgs e)
        {
            subsidyProvider = empRepo.GetSubsidyProviderByKey(Convert.ToInt32(e.Key));
            _rebindAddress = true;

            if (PrivateCacheData.ContainsKey(SubsidyProviderAddress))
                PrivateCacheData.Remove(SubsidyProviderAddress);

            if (PrivateCacheData.ContainsKey(SelectedSubsidyProvider))
                PrivateCacheData[SelectedSubsidyProvider] = subsidyProvider;
            else
                PrivateCacheData.Add(SelectedSubsidyProvider, subsidyProvider);
        }

        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            // Add exclusion set
            this.ExclusionSets.Add(RuleExclusionSets.LegalEntityLeadApplicants);

            if (PrivateCacheData.ContainsKey(SelectedSubsidyProvider) && PrivateCacheData[SelectedSubsidyProvider] != null)
            {
                subsidyProvider = PrivateCacheData[SelectedSubsidyProvider] as SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider;
            }

            if (PrivateCacheData.ContainsKey(SubsidyProviderAddress) && PrivateCacheData[SubsidyProviderAddress] != null)
            {
                address = PrivateCacheData[SubsidyProviderAddress] as SAHL.Common.BusinessModel.Interfaces.IAddress;
            }

            if (subsidyProvider != null)
                subsidyProvider = empRepo.GetSubsidyProviderByKey(subsidyProvider.Key);
            
            if (address != null)
                address = addressRepository.GetAddressByKey(address.Key);

            // Set object to default value
            if (address == null)
                address = _view.GetCapturedAddress;
           
            ILegalEntityRepository leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            
            if (subsidyProvider != null)
            {
                SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider subsidyProviderUpdated = _view.GetCapturedSubsidyProvider(subsidyProvider);

                TransactionScope txn = new TransactionScope();

                try
                {
                    // Get address type
                    IAddressType addressType = null;
                    if (_view.SelectedAddressTypeValue != SAHLDropDownList.PleaseSelectValue && _view.SelectedAddressFormatValue != SAHLDropDownList.PleaseSelectValue)
                        addressType = addressRepository.GetAddressTypeByKey(Int32.Parse(_view.SelectedAddressTypeValue));

                    // Update LE Details
                    // TODO // leRepo.SaveLegalEntity(subsidyProviderUpdated.LegalEntity, false);
                    // Update Subsidy Provider Details
                    empRepo.SaveSubsidyProvider(subsidyProviderUpdated);

                    if (addressType != null)
                    {
                        // Add new Address
                        if (subsidyProviderUpdated.LegalEntity.LegalEntityAddresses == null || subsidyProviderUpdated.LegalEntity.LegalEntityAddresses.Count == 0)
                        {
                            address = _view.GetCapturedAddress;
                            address = _view.GetCapturedSubsidyProviderAddress(address);
                            leRepo.SaveAddress(addressType, subsidyProvider.LegalEntity, address, _view.EffectiveDate);
                        }
                        else
                        {
                            if (address.GetType().Name == subsidyProviderUpdated.LegalEntity.LegalEntityAddresses[0].Address.GetType().Name)
                            {
                                // Has address but updating current address
                                address = subsidyProviderUpdated.LegalEntity.LegalEntityAddresses[0].Address;
                                address = _view.GetCapturedSubsidyProviderAddress(address);
                                leRepo.SaveAddress(addressType, subsidyProviderUpdated.LegalEntity, address, _view.EffectiveDate);
                            }
                            else
                            {
                                // Different address format, have to create new address
                                address = _view.GetCapturedAddress;
                                address = _view.GetCapturedSubsidyProviderAddress(address);
                                leRepo.SaveAddress(addressType, subsidyProviderUpdated.LegalEntity, address, _view.EffectiveDate);
                            }
                        }
                    }
                    this.ExclusionSets.Remove(RuleExclusionSets.LegalEntityLeadApplicants);

                    if (PrivateCacheData.ContainsKey(SelectedSubsidyProvider))
                        PrivateCacheData[SelectedSubsidyProvider] = subsidyProviderUpdated;
                    else
                        PrivateCacheData.Add(SelectedSubsidyProvider, subsidyProviderUpdated);

                    txn.VoteCommit();
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

                if (_view.Messages.Count == 0)
                    Navigator.Navigate("SubsidyProviderDetails");
            }

        }

    }
}
