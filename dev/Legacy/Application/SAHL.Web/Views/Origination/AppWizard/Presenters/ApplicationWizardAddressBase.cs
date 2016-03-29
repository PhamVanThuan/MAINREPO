using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;

using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Origination.Presenters
{
    public class ApplicationWizardAddressBase : SAHLCommonBasePresenter<IAddressView>
    {
        #region Private / Protected Attributes

        private ILookupRepository _lookupRepository;
        private ILegalEntity _legalEntity;
        private IApplication _application;
        private IAddressRepository _addressRepository;
        private ILegalEntityRepository _legalEntityRepository;
        private IApplicationRepository _applicationRepository;
        IEventList<ILegalEntityAddress> _lstLegalEntityAddresses = new EventList<ILegalEntityAddress>();
        private int _addressType = -1;
        

        #endregion

        #region Constructor

        public ApplicationWizardAddressBase(IAddressView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        protected IAddressRepository AddressRepository
        {
            get
            {
                if (_addressRepository == null)
                    _addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();
                return _addressRepository;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected int AddressType
        {
            get
            {
                return _addressType;
            }
            set
            {
                _addressType = value;
            }

        }
        

        /// <summary>
        /// 
        /// </summary>
        protected IApplicationRepository ApplicationRepository
        {
            get
            {
                if (_applicationRepository == null)
                    _applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
                return _applicationRepository;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected ILegalEntityRepository LegalEntityRepository
        {
            get
            {
                if (_legalEntityRepository == null)
                    _legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                return _legalEntityRepository;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected ILookupRepository LookupRepository
        {
            get
            {
                if (_lookupRepository == null)
                    _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                return _lookupRepository;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected ILegalEntity LegalEntity
        {
            get
            {
                return _legalEntity;
            }
            set
            {
                _legalEntity = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected IEventList<ILegalEntityAddress> ListLegalEntityAddresses
        {
            get
            {
                return _lstLegalEntityAddresses;
            }
            set
            {
                _lstLegalEntityAddresses = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected IApplication Application
        {
            get
            {
                return _application;
            }
            set
            {
                _application = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears all items out of the cache.
        /// </summary>
        protected void ClearCachedData()
        {
            GlobalCacheData.Remove(ViewConstants.NavigateTo);
            GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;
            base.OnViewInitialised(sender, e);
            _view.GridAddressSelectedIndexChanged += new EventHandler(_view_GridAddressSelectedIndexChanged);
                       
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_GridAddressSelectedIndexChanged(object sender, EventArgs e)
        {
            ILegalEntityAddress address = _view.SelectedAddress as ILegalEntityAddress;
            bool found = false;

           string addressTypeDesc = LookupRepository.AddressTypes[_addressType];

           IDictionary<int,string> formats = null;

           switch (_addressType)
           {
               case (int)SAHL.Common.Globals.AddressTypes.Postal:
                   {
                       formats =_lookupRepository.AddressFormatsByAddressType(AddressTypes.Postal);
                       break;
                   }
               case (int)SAHL.Common.Globals.AddressTypes.Residential:
                   {
                       formats = _lookupRepository.AddressFormatsByAddressType(AddressTypes.Residential);
                       break;
                   }
           }
                        
           foreach (KeyValuePair<int,string> kvp in formats)
           {
               if (kvp.Key == address.Address.AddressFormat.Key)
               {                   
                   found = true;                  
                   _view.SetAddress(address.Address);
               }
           }

            if (found == false)
            {

                _view.Messages.Add(new Error("Cannot select an incompatible address type. " + address.Address.AddressFormat.Description + " format is not compatible with address type " + addressTypeDesc, "Cannot select an incompatible address type. " + address.Address.AddressFormat.Description + " format is not compatible with address type " + addressTypeDesc));
                _view.AddressFormat = AddressFormats.Street;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetGridSelectedIndex()
        {
            if (_addressType == -1)
            {
                throw new Exception("ApplicationWizardAddressBase : AddressType must be populated");                
            }

            // make sure the grid allows post back - this needs to happen BEFORE the bind event because of the way the grid is 
            // implemented
            _view.GridPostBack = true;

            _view.PopulateInputFromGrid = false;

             _view.ShowLegalEntityNameOnAddressGrid = true;

            int legalEntityKey = -1;
            int applicationKey = -1;
            // check the global data cache for the legal entity key
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
                legalEntityKey = Convert.ToInt32(GlobalCacheData[ViewConstants.SelectedLegalEntityKey]);

            // check the global data cache for the application key
            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
                applicationKey = Convert.ToInt32(GlobalCacheData[ViewConstants.ApplicationKey]);


            _legalEntity = LegalEntityRepository.GetLegalEntityByKey(legalEntityKey);
            _application = ApplicationRepository.GetApplicationByKey(applicationKey);

            _view.LegalEntity = _legalEntity;

            PopulateAddressList();

            if (!_view.IsPostBack)
            {
                bool found = false;
                if (LegalEntity.LegalEntityAddresses.Count > 0)
                {
                    for (int x = 0; x < _lstLegalEntityAddresses.Count; x++)
                    {
                        if (_lstLegalEntityAddresses[x].LegalEntity == _legalEntity 
                            && _lstLegalEntityAddresses[x].AddressType.Key == _addressType)
                        {
                            _view.AddressFormat = (AddressFormats)_lstLegalEntityAddresses[x].Address.AddressFormat.Key;
                            _view.SetAddress(_lstLegalEntityAddresses[x]);
                            _view.GridSelectedIndex = x;
                            found = true;
                            break;
                        }
                    }
                }
                if (found)
                {
                    _view.AddButtonText = "Update";
                }
                else
                {
                    _view.AddButtonText = "Add";
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void PopulateAddressList()
        {
            IReadOnlyEventList<IApplicationRole> roles = _application.GetApplicationRolesByGroup(SAHL.Common.Globals.OfferRoleTypeGroups.Client);
            for (int x = 0; x < roles.Count; x++)
            {
                foreach (ILegalEntityAddress address in roles[x].LegalEntity.LegalEntityAddresses)
                {
                    //if (!address.LegalEntity.DisplayName.ToLower().Contains("branchconsultantuser"))
                    //{
                    if (address.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
                        _lstLegalEntityAddresses.Add(_view.Messages, address);
                    //}
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void BindLegalEntityAddresses()
        {           
            _view.BindAddressList(_lstLegalEntityAddresses);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        protected void Save(IAddress address)
        {
            TransactionScope ts = new TransactionScope();
            {
                try
                {
                    IAddressType addressType = AddressRepository.GetAddressTypeByKey(_addressType);
                    DateTime effectiveDate = DateTime.Today;
                    if (_view.EffectiveDate.HasValue && _view.EffectiveDate.Value > DateTime.Today)
                    {
                        effectiveDate = _view.EffectiveDate.Value;
                    }

                    //if an existing address of the addressType exists then update it - want only one
                    foreach (ILegalEntityAddress leAddress in _legalEntity.LegalEntityAddresses)
                    {
                        if (leAddress.AddressType.Key == _addressType)
                        {
                            leAddress.Address = address;
                            LegalEntityRepository.SaveLegalEntityAddress(leAddress, address);
                            bool foundPostalAddress = false;
                            foreach (ILegalEntityAddress add in _legalEntity.LegalEntityAddresses)
                            {
                                if (add.AddressType.Key == (int)SAHL.Common.Globals.AddressTypes.Postal
                                    && add.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
                                {
                                    foundPostalAddress = true;
                                    break;
                                }
                            }

                            if (!foundPostalAddress)
                            {
                                int postaladdressTypeKey = (int)SAHL.Common.Globals.AddressTypes.Postal;
                                IAddressType at = AddressRepository.GetAddressTypeByKey(postaladdressTypeKey);
                                LegalEntityRepository.SaveAddress(at, _legalEntity, address, effectiveDate);
                            }
                            return;
                        }
                    }

                    LegalEntityRepository.SaveAddress(addressType, _legalEntity, address, effectiveDate);

                    //Automatically add the postal address first time around if the address type is not free text
                    if (_addressType == (int)SAHL.Common.Globals.AddressTypes.Residential
                        && _view.AddressFormat != SAHL.Common.Globals.AddressFormats.FreeText)
                    {

                        bool foundPostalAddress = false;
                        foreach (ILegalEntityAddress leAddress in _legalEntity.LegalEntityAddresses)
                        {
                            if (leAddress.AddressType.Key == (int)SAHL.Common.Globals.AddressTypes.Postal
                                && leAddress.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
                            {
                                foundPostalAddress = true;
                                break;
                            }
                        }

                        if (!foundPostalAddress)
                        {
                            int postaladdressTypeKey = (int)SAHL.Common.Globals.AddressTypes.Postal;
                            IAddressType at = AddressRepository.GetAddressTypeByKey(postaladdressTypeKey);
                            LegalEntityRepository.SaveAddress(at, _legalEntity, address, effectiveDate);
                        }
                    }
                    ts.VoteCommit();
                }
                catch (Exception)
                {
                    ts.VoteRollBack();
                    if (_view.GetCapturedAddress().GetFormattedDescription(AddressDelimiters.Comma) == null
                        || _view.GetCapturedAddress().GetFormattedDescription(AddressDelimiters.Comma).Length == 0)
                    {
                        _view.Messages.Add(new Error("Please specify an address before continuing.", "Please specify an address before continuing."));
                    }
                   
                    if (_view.IsValid)
                        throw;
                }
                finally
                {
                    ts.Dispose();
                }
            }
        }
        #endregion

        
    }
}
