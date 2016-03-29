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
using Castle.ActiveRecord;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.Address
{
    /// <summary>
    /// Presenter used for deleting a legal entity's addresses.
    /// </summary>
    public class LegalEntityAddressDelete : LegalEntityAddressBase
    {

        public LegalEntityAddressDelete(IAddressView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            // add event handlers
            _view.CancelButtonClicked += new EventHandler(_view_CancelButtonClicked);
            _view.DeleteButtonClicked += new EventHandler(_view_DeleteButtonClicked);

        }

        void _view_CancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        void _view_DeleteButtonClicked(object sender, EventArgs e)
        {
            ILegalEntityAddress leAddress = View.SelectedAddress as ILegalEntityAddress;
            TransactionScope txn = new TransactionScope();

            try
            {
                if (leAddress != null)
                {
                    IRuleService ruleService = ServiceFactory.GetService<IRuleService>();
                    ruleService.ExecuteRule(_view.Messages, "LegalEntityAddressCanNotBeDeletedIfItIsTheActiveLegalEntityDomicilium", leAddress);

                    LegalEntityRepository.DeleteAddress(leAddress);
                }
                else
                {
                    // in the case of failed addresses, just mark it as cleaned
                    IFailedLegalEntityAddress dirtyAddress = View.SelectedAddress as IFailedLegalEntityAddress;
                    AddressRepository.CleanDirtyAddress(dirtyAddress);
                }
                txn.VoteCommit();
                _view.Navigator.Navigate("LegalEntityAddressDisplay");
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

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return; 

            // set panel display properties
            _view.AddressListHeaderText = "Legal Entity Addresses";
            _view.AddressListVisible = true;
            _view.AddressDetailsVisible = true;

            // make buttons visible
            _view.CancelButtonVisible = true;
            _view.DeleteButtonVisible = (_view.SelectedAddress != null);
        }

    }
}
