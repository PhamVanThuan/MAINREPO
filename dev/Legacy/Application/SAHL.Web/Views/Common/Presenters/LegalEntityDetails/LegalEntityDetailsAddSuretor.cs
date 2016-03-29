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
using SAHL.Web.Views.Common.Presenters.LegalEntityDetails;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    public class LegalEntityDetailsAddSuretor : LegalEntityDetailsAddBase
    {
        public LegalEntityDetailsAddSuretor(ILegalEntityDetails View, SAHLCommonBaseController Controller)
            : base(View, Controller)
        {

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return; 

            View.CancelButtonVisible = false;
            View.SubmitButtonText = "Add";
        }

        protected override void SubmitButtonClicked(object sender, EventArgs e)
        {

            // TODO: Attempt to save
            base.SaveLegalEntity();

            base.SubmitButtonClicked(sender, e);

        }
    }
}
