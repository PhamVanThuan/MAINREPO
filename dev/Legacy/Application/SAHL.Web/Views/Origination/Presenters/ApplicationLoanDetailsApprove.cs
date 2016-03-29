using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.Common.Web.UI;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Origination.Presenters
{
    public class ApplicationLoanDetailsApprove : ApplicationLoanDetailsBase
    {
        public ApplicationLoanDetailsApprove(IApplicationLoanDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.OnUpdateClicked += new EventHandler(OnUpdateClicked);
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            if (!_view.ShouldRunPage)
                return;
            // Hide the readonly controls and show the editable ones
            _view.IsReadOnly = true;
            _view.IsPropertyValueReadOnly = true;
            _view.IsQuickCashDetailsReadOnly = false;
            // QC not available on New Purchases
            if (Application.ApplicationType.Key == (int)OfferTypes.NewPurchaseLoan)
                _view.IsQuickCashVisible = false;
            else
                _view.IsQuickCashVisible = true;

            _view.IsButtonsPanelReadonly = false;
            _view.CanShowQuickCash = true;
            _view.ShowCalculateButton = false;

            //DoITCCheck();
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {

            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            _view.IsDiscountReadonly = true;

            GetApplicationFromCBO();

            BindLookups();

            //Must be called before BindApplicationDetails
            CheckQCReasons();

            // Bind the Application
            _view.BindApplicationDetails(Application);
            _view.BindProduct(base.LookupRepository.Products.BindableDictionary[Convert.ToString((int)Application.CurrentProduct.ProductType)]);
            _view.SetSubmitCaption("Approve Application");
        }
    }
}
