using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Origination.Presenters
{
    public class ApplicationLoanDetailsDisplay : ApplicationLoanDetailsBase
    {
        public ApplicationLoanDetailsDisplay(IApplicationLoanDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            if (!_view.ShouldRunPage)
                return;


            // Hide the editable controls and show the read-only ones
            _view.IsReadOnly = true;
            _view.IsPropertyValueReadOnly = true;
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            GetApplicationFromCBO();

            BindLookups();

            // Bind the Application
            _view.BindApplicationDetails(Application);
            _view.BindProduct(base.LookupRepository.Products.BindableDictionary[Convert.ToString((int)Application.CurrentProduct.ProductType)]);
        }
    }
}
