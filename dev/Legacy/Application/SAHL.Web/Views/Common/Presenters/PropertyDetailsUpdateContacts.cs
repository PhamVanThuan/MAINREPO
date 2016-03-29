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
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Common.Presenters
{
    public class PropertyDetailsUpdateContacts : PropertyDetailsBase
    {
        /// <summary>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PropertyDetailsUpdateContacts(IPropertyDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // setup screen mode
            _view.PropertyDetailsUpdateMode = PropertyDetailsUpdateMode.Contact;
        }


        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            // setup events
            _view.OnUpdateButtonClicked += new EventHandler(OnUpdateButtonClicked);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.ShowPropertyGrid = true;
            _view.ShowDeedsTransfersGrid = false;

            if (!ValidateOnViewInitialised())
                _view.ButtonRowVisible = false;
        }

        void OnUpdateButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();

            IProperty updatedProperty = _view.SelectedProperty;

            try
            {
                if (updatedProperty.PropertyAccessDetails == null)
                {
                    // populate the new property access details
                    IPropertyAccessDetails propertyAccessDetails = PropertyRepo.CreateEmptyPropertyAccessDetails();
                    propertyAccessDetails.Property = _view.SelectedProperty;
                    propertyAccessDetails.Contact1 = _view.UpdatedContactName;
                    propertyAccessDetails.Contact1Phone = _view.UpdatedContactNumber;
                    propertyAccessDetails.Contact2 = _view.UpdatedContactName2;
                    propertyAccessDetails.Contact2Phone = _view.UpdatedContactNumber2;
                    updatedProperty.PropertyAccessDetails = propertyAccessDetails;
                }
                else
                {
                    // populate the new property access details
                    updatedProperty.PropertyAccessDetails.Contact1 = _view.UpdatedContactName;
                    updatedProperty.PropertyAccessDetails.Contact1Phone = _view.UpdatedContactNumber;
                    updatedProperty.PropertyAccessDetails.Contact2 = _view.UpdatedContactName2;
                    updatedProperty.PropertyAccessDetails.Contact2Phone = _view.UpdatedContactNumber2;
                }

                // save the property
                PropertyRepo.SaveProperty(updatedProperty);

                txn.VoteCommit();

                _view.Navigator.Navigate("Update");
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
}
