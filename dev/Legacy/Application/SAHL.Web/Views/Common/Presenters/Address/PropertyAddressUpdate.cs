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
using SAHL.Common.Collections;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.UI;
using Castle.ActiveRecord;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Exceptions;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Helpers;


namespace SAHL.Web.Views.Common.Presenters.Address
{
    public class PropertyAddressUpdate : SAHLCommonBasePresenter<IAddressView>
    {
        private ILookupRepository _lookupRepository;
        private IProperty _property;
        private IAddressRepository _addressRepository;
        private IPropertyRepository _propertyRepository;
        private IEventList<IAddress> _addresses;
        private CBOMenuNode _node;
        private IApplicationMortgageLoan _applicationMortgageLoan;
        private IApplicationRepository _appRepo;

        private bool _excludePropertyAddressUpdateRules;
        public bool ExcludePropertyAddressUpdateRules
        {
            get { return _excludePropertyAddressUpdateRules; }
            set { _excludePropertyAddressUpdateRules = value; }
        }

        public PropertyAddressUpdate(IAddressView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view = view;
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            int propertyKey = 0;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            if (_propertyRepository == null)
                _propertyRepository = RepositoryFactory.GetRepository<IPropertyRepository>();
            if (_lookupRepository == null)
                _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
            if (_addressRepository == null)
                _addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();
            if (_appRepo == null)
                _appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            // check the node type and get the property key
            switch (_node.GenericKeyTypeKey)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                    // get the account
                    IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                    IMortgageLoanAccount mortgageLoanAccount = accRepo.GetAccountByKey(_node.GenericKey) as IMortgageLoanAccount;
                    if (mortgageLoanAccount != null)
                    {
                        if (mortgageLoanAccount.SecuredMortgageLoan.Property != null)
                            propertyKey = mortgageLoanAccount.SecuredMortgageLoan.Property.Key;
                    }
                    break;
                case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
                    // get the application
                    _applicationMortgageLoan = _appRepo.GetApplicationByKey(_node.GenericKey) as IApplicationMortgageLoan;
                    if (_applicationMortgageLoan != null)
                    {
                        if (_applicationMortgageLoan.Property != null)
                            propertyKey = _applicationMortgageLoan.Property.Key;
                    }
                    break;
                case (int)SAHL.Common.Globals.GenericKeyTypes.Property:
                    propertyKey = _node.GenericKey;
                    if (_node.ParentNode.GenericKeyTypeKey == (int)GenericKeyTypes.Offer)
                        _applicationMortgageLoan = _appRepo.GetApplicationByKey(_node.ParentNode.GenericKey) as IApplicationMortgageLoan;
                    break;
                default:
                    break;
            }

            // button event handlers
            _view.CancelButtonClicked += new EventHandler(OnCancelButtonClicked);
            _view.UpdateButtonClicked += new EventHandler(OnUpdateButtonClicked);
            _view.ExistingAddressSelected += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(OnExistingAddressSelected);

            // get the selected property
            _property = _propertyRepository.GetPropertyByKey(propertyKey);

            // get the address for the property
            _addresses = new EventList<IAddress>();
            _addresses.Add(_view.Messages, _property.Address);

            // bond the addresses
            _view.BindAddressList(_addresses);

            // bind lookup data
            _view.BindAddressStatuses(_lookupRepository.GeneralStatuses.Values);
            _view.BindAddressTypes(_lookupRepository.AddressTypes);

            _excludePropertyAddressUpdateRules = true;

        }
        void OnExistingAddressSelected(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            IAddress address = _addressRepository.GetAddressByKey(Convert.ToInt32(e.Key));
            SaveAndNavigate(address, "Update");
        }

        void OnUpdateButtonClicked(object sender, EventArgs e)
        {
            IAddress address = _view.GetCapturedAddress();
            SaveAndNavigate(address, "Update");
        }

        void OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

            // set panel display properties
            _view.AddressListHeaderText = "Property Addresses";
            _view.AddressListVisible = true;
            _view.AddressFormVisible = true;
            _view.AddressStatusVisible = true;
            _view.LegalEntityInfoVisible = false;
            _view.AddressFormatReadOnly = true;
            _view.AddressTypeReadOnly = true;

            _view.GridColumnStatusVisible = false;
            _view.GridColumnTypeVisible = false;
            _view.GridColumnEffectiveDateVisible = false;

            // bind address formats - also needs to be done late as addresses change when the grid is selected
            _view.BindAddressFormats(_lookupRepository.AddressFormatsByAddressType((AddressTypes)Int32.Parse(_view.SelectedAddressTypeValue)));

            // make buttons visible
            _view.CancelButtonVisible = true;
            _view.UpdateButtonVisible = true;
        }

        private void SaveAndNavigate(IAddress address, string navigateValue)
        {
            // exclude the relevant rules
            if (_excludePropertyAddressUpdateRules)
                this.ExclusionSets.Add(RuleExclusionSets.PropertyAddressUpdateView);

            TransactionScope txn = new TransactionScope();

            try
            {
                //SAHL.Common.BusinessModel.Helpers.Extensions.GetPreviousValue(_property,);
                IAddress previous = _property.GetPreviousValue<IProperty, IAddress>(p => p.Address);

                IRuleService ruleService = ServiceFactory.GetService<IRuleService>();
                ruleService.ExecuteRule(_view.Messages, "WhenChangingPropertyAddressDetailsWarnUserOfLegalEntitiesUsingAsDomicilium", previous);
                _propertyRepository.SaveAddress(_property, address);

                txn.VoteCommit();

                if (_view.IsValid)
                {
                    //check if we have a parent instance node and refresh the instance node to reflect the new address description
                    bool parentInstanceNodeExists = CBOManager.CheckForParentNodeByType(_view.CurrentPrincipal, _node, typeof(InstanceNode));
                    if (parentInstanceNodeExists)
                        CBOManager.RefreshInstanceNode(_view.CurrentPrincipal);

                    _view.Navigator.Navigate(navigateValue);
                }
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

        protected bool ValidateOnViewInitialised()
        {
            if (_property != null && _applicationMortgageLoan != null)
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                IRuleService svc = ServiceFactory.GetService<IRuleService>();
                svc.ExecuteRule(spc.DomainMessages, "PropertyNoUpdateOnOpenLoan", _property, _applicationMortgageLoan);
            }

            if (_view.Messages.Count == 0)
                return true;
            else
                return false;
        }

    }
}
