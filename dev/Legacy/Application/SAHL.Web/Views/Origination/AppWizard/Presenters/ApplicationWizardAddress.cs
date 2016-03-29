using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.DomainMessages;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Web.Views.Origination.Presenters
{
    public class ApplicationWizardAddress : ApplicationWizardAddressBase
    {


        public ApplicationWizardAddress(IAddressView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }


        protected override void OnViewInitialised(object sender, EventArgs e)
        {

            if (_view.IsMenuPostBack)
                ClearCachedData();

            if (!_view.ShouldRunPage) return;

            AddressType = (int)SAHL.Common.Globals.AddressTypes.Residential;

            SetGridSelectedIndex();

            BindLegalEntityAddresses();

            base.OnViewInitialised(sender, e);

            // set up event handlers
            _view.SelectedAddressTypeChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_SelectedAddressTypeChanged);
            _view.ExistingAddressSelected += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_ExistingAddressSelected);


            // bind lookup data
            _view.BindAddressTypes(LookupRepository.AddressTypes);
            _view.BindAddressFormats(LookupRepository.AddressFormatsByAddressType((AddressTypes)(SAHL.Common.Globals.AddressTypes.Residential)));

            // button event handlers
            _view.UpdateButtonClicked += new EventHandler(_view_UpdateButtonClicked);
            _view.CancelButtonClicked += new EventHandler(_view_CancelButtonClicked);
            _view.BackButtonClicked += new EventHandler(_view_BackButtonClicked);

        }

        void _view_BackButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Back");
        }

        void _view_SelectedAddressTypeChanged(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            _view.BindAddressFormats(LookupRepository.AddressFormatsByAddressType((AddressTypes)Convert.ToInt32(e.Key)));
        }

        void _view_UpdateButtonClicked(object sender, EventArgs e)
        {
            Save(_view.GetCapturedAddress());
            if (_view.IsValid)
                _view.Navigator.Navigate("Next");
        }

        void _view_ExistingAddressSelected(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            IAddress address = AddressRepository.GetAddressByKey(Convert.ToInt32(e.Key));
            Save(address);
            if (_view.IsValid)
                _view.Navigator.Navigate("Next");
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
            base.OnViewPreRender(sender, e);

            _view.AddressTypeReadOnly = true;

            _view.AddressType = SAHL.Common.Globals.AddressTypes.Residential;

            _view.UpdateButtonText = "Next";

            // set panel display properties
            _view.AddressListHeaderText = "Legal Entity Addresses";
            _view.AddressListVisible = true;
            _view.AddressFormVisible = true;

            // make buttons visible
            _view.CancelButtonVisible = true;
            _view.UpdateButtonVisible = true;
            _view.BackButtonVisible = true;
        }
    }
}

  
