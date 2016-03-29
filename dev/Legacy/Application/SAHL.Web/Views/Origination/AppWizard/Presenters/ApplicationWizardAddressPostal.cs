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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Origination.Presenters
{
    public class ApplicationWizardAddressPostal : ApplicationWizardAddressBase
    {

        public ApplicationWizardAddressPostal(IAddressView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage) return;

            AddressType = (int)SAHL.Common.Globals.AddressTypes.Postal;

            SetGridSelectedIndex();

            BindLegalEntityAddresses();           

            base.OnViewInitialised(sender, e);

          
            // set up event handlers
            _view.SelectedAddressTypeChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_SelectedAddressTypeChanged);
            _view.ExistingAddressSelected += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_ExistingAddressSelected);

            // bind lookup data
            _view.BindAddressTypes(LookupRepository.AddressTypes);
            _view.BindAddressFormats(LookupRepository.AddressFormatsByAddressType(AddressTypes.Postal));

            // button event handlers
            _view.UpdateButtonClicked += new EventHandler(_view_UpdateButtonClicked);
            _view.CancelButtonClicked += new EventHandler(_view_CancelButtonClicked);
            _view.BackButtonClicked += new EventHandler(_view_BackButtonClicked);
                       
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_BackButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Back");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_SelectedAddressTypeChanged(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            _view.BindAddressFormats(LookupRepository.AddressFormatsByAddressType((AddressTypes)Convert.ToInt32(e.Key)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_UpdateButtonClicked(object sender, EventArgs e)
        {
            
            Save(_view.GetCapturedAddress());
            if (_view.IsValid)
            {
                _view.Navigator.Navigate("Finish");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_ExistingAddressSelected(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            IAddress address = AddressRepository.GetAddressByKey(Convert.ToInt32(e.Key));
            Save(address);
            if (_view.IsValid)
            {
                _view.Navigator.Navigate("Finish");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            _view.UpdateButtonText = "Finish";

            // set panel display properties
            _view.AddressListHeaderText = "Legal Entity Addresses";
            _view.AddressListVisible = true;
            _view.AddressFormVisible = true;

            // make buttons visible
            _view.CancelButtonVisible = true;
            _view.UpdateButtonVisible = true;
            _view.BackButtonVisible = true;

            _view.AddressTypeReadOnly = true;

            _view.AddressType = SAHL.Common.Globals.AddressTypes.Postal;
           
        }

    }
}
