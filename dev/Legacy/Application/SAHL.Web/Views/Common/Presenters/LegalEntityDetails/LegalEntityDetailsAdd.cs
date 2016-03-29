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

namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    public class LegalEntityDetailsAdd: LegalEntityDetailsAddBase
    {
        public LegalEntityDetailsAdd(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return; 

            View.CancelButtonVisible = true;
            View.SubmitButtonText = "Update";
        }

        protected override void SubmitButtonClicked(object sender, EventArgs e)
        {
            // TODO: Attempt to save
            SaveLegalEntity();

            base.SubmitButtonClicked(sender, e);
        }
    }
}
