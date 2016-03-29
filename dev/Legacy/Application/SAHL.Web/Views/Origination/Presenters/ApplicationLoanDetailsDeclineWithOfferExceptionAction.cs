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
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Origination.Presenters
{
    public class ApplicationLoanDetailsDeclineWithOfferExceptionAction : ApplicationLoanDetailsBase
    {
        public ApplicationLoanDetailsDeclineWithOfferExceptionAction(IApplicationLoanDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.OnUpdateClicked += new EventHandler(OnUpdateRevisionClicked);
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            if (!_view.ShouldRunPage)
                return;


            // Hide the readonly controls and show the editable ones
            _view.IsQuickPayFeeReadOnly = false;
            _view.IsReadOnly = true;
            _view.IsPropertyValueReadOnly = true;
            _view.IsQuickCashDetailsReadOnly = false;
            _view.IsButtonsPanelReadonly = false;
            _view.IsSPVReadOnly = false;
            //This presenter enables the Bond to Register and allows it to be the 
            //same as the LAA
            _view.IsBondToRegisterExceptionAction = true;

            // QC not available on New Purchases
            if (Application.ApplicationType.Key == (int)SAHL.Common.Globals.OfferTypes.NewPurchaseLoan)
                _view.IsQuickCashVisible = false;
            else
                _view.IsQuickCashVisible = true;

            //except for VF, discount must be editable
            //if (Application.CurrentProduct.ProductType != SAHL.Common.Globals.Products.VariFixLoan)
                _view.IsDiscountReadonly = false;

            _view.IsCashDepositReadOnly = false;
            _view.IsCashOutReadOnly = false;
            _view.CanShowQuickCash = true;
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {

            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            GetApplicationFromCBO();

            BindLookups();

            //Must be called before BindApplicationDetails
            CheckQCReasons();

            // Bind the Application
            _view.BindApplicationDetails(Application);
            _view.BindProduct(base.LookupRepository.Products.BindableDictionary[Convert.ToString((int)Application.CurrentProduct.ProductType)]);
            _view.SetSubmitCaption("Decline with offer");
            _view.CreateRevision = true;

        }
    }
}
