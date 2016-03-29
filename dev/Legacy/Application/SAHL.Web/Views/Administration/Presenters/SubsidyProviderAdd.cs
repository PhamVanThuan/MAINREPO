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
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.Factories;
using SAHL.Common.Exceptions;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.Administration.Presenters
{
    /// <summary>
    /// Subsidy Provider - Add
    /// </summary>
    public class SubsidyProviderAdd : SubsidyProviderBase
    {
        /// <summary>
        /// Selected subsidy provider address - used in private cache
        /// </summary>
        const string SubsidyProviderAddress = "SelectedSusbidyProviderAddress";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public SubsidyProviderAdd(SAHL.Web.Views.Administration.Interfaces.ISubsidyProvider view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!View.ShouldRunPage) return;

            // if the Private Cache has subsidy data or address when the page first loads - clear cache !
            if (!_view.IsPostBack && PrivateCacheData.ContainsKey(SelectedSubsidyProvider) && PrivateCacheData[SelectedSubsidyProvider] != null)
                PrivateCacheData[SelectedSubsidyProvider] = null;

            if (!_view.IsPostBack && PrivateCacheData.ContainsKey(SubsidyProviderAddress) && PrivateCacheData[SubsidyProviderAddress] != null)
                PrivateCacheData[SubsidyProviderAddress] = null;

            _view.SetButtonVisibility = true;
            _view.SetControlsForUpdate = false;

            _view.BindDropDowns(lookups);

            _view.AddAddressButtonClicked += new KeyChangedEventHandler(_view_AddAddressButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
            _view.OnReBindSubsidyDetails+=new KeyChangedEventHandler(_view_OnReBindSubsidyDetails);
        }
       /// <summary>
       /// OnPreRender event
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>

        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            // if the subsidy provider exists, make the screen readonly 
            if (PrivateCacheData.ContainsKey(SelectedSubsidyProvider) && PrivateCacheData[SelectedSubsidyProvider] != null)
                subsidyProvider = PrivateCacheData[SelectedSubsidyProvider] as SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider;

            if (subsidyProvider == null)
                _view.SetControlsForUpdate = false;
            else
            {
                _view.EnableDisableUpdate = false;
                _view.SetControlsForDisplay = false;
                _view.SetControlsForUpdate = true;
                _view.BindSubsidyProviderDetail(subsidyProvider);
            }

            if (PrivateCacheData.ContainsKey(SubsidyProviderAddress) && PrivateCacheData[SubsidyProviderAddress] != null)
                address = PrivateCacheData[SubsidyProviderAddress] as SAHL.Common.BusinessModel.Interfaces.IAddress;

            if (_view.IsPostBack && address != null)
                _view.BindSubsidyProviderAddress(address);

            // When Initially loading the page, make addressDetails invisible by default
            if (!_view.IsPostBack && subsidyProvider == null)
            {
                _view.AddressDetailsVisible = false;
            }

            // On each postback test either drop down for "-select-" value selected, in which case, 
            // address control should be invisible.
            if ( _view.IsPostBack &&
               (_view.SelectedAddressTypeValue == "-select-" || _view.SelectedAddressFormatValue == "-select-"))
            {
                _view.AddressDetailsVisible = false;
            }

            //Set Default Address Type To Postal as it is the only Address Type in the drop down list
            _view.SetAddressType = (int)AddressTypes.Postal;

            //Set default date as current date
            _view.EffectiveDate = DateTime.Today;

            _view.SubsidyProviderAjaxEnabled = false;
        }

        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            // Add exclusion set
            this.ExclusionSets.Add(RuleExclusionSets.LegalEntityLeadApplicants);

            SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider subsidyProviderNew = empRepo.CreateEmptySubsidyProvider();
            ILegalEntityRepository leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

            subsidyProviderNew = _view.GetSusbidyProviderForAdd(subsidyProviderNew);

            IAddressType addressType = null;

            if (_view.SelectedAddressTypeValue != SAHLDropDownList.PleaseSelectValue && _view.SelectedAddressFormatValue != SAHLDropDownList.PleaseSelectValue)
               addressType = addressRepository.GetAddressTypeByKey(Int32.Parse(_view.SelectedAddressTypeValue));
   
            if (PrivateCacheData.ContainsKey(SubsidyProviderAddress) && PrivateCacheData[SubsidyProviderAddress] != null)
                 address = PrivateCacheData[SubsidyProviderAddress] as SAHL.Common.BusinessModel.Interfaces.IAddress;

            IAddress addressNew;
            // if address was selected on screen - use selected address
            if (address != null)
                addressNew = address;
            else
                addressNew = _view.GetCapturedAddress;

            TransactionScope txn = new TransactionScope();

            try
            {
                leRepo.SaveLegalEntity(subsidyProviderNew.LegalEntity, false);

                if (addressType != null)
                    leRepo.SaveAddress(addressType, subsidyProviderNew.LegalEntity, addressNew, _view.EffectiveDate);

                empRepo.SaveSubsidyProvider(subsidyProviderNew);

                this.ExclusionSets.Remove(RuleExclusionSets.LegalEntityLeadApplicants);
                txn.VoteCommit();
           }

            catch (Exception)
            {
                txn.VoteRollBack();
                if (View.IsValid)
                    throw;
            }
            finally
            {
                txn.Dispose();
            }

            if (_view.Messages.Count == 0)
                Navigator.Navigate("SubsidyProviderDetails");
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
        /// Rebind subsidy details - throw error if susbidy provider exists
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _view_OnReBindSubsidyDetails(object sender, KeyChangedEventArgs e)
        {
            subsidyProvider = empRepo.GetSubsidyProviderByKey(Convert.ToInt32(e.Key));

            if (PrivateCacheData.ContainsKey(SelectedSubsidyProvider))
                PrivateCacheData[SelectedSubsidyProvider] = subsidyProvider;
            else
                PrivateCacheData.Add(SelectedSubsidyProvider, subsidyProvider);

        }
    }
}
