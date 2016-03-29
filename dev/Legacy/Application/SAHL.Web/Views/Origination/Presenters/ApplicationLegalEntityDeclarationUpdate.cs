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
using SAHL.Web.Views.Origination.Interfaces;


namespace SAHL.Web.Views.Origination.Presenters
{
    public class ApplicationLegalEntityDeclarationUpdate : ApplicationLegalEntityDeclarationDisplay
    {
        public ApplicationLegalEntityDeclarationUpdate(IApplicationLegalEntityDeclaration view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            // set update mode
            _view.UpdateMode = true;
            _view.UpdateButtonText = "Update";

            // call the base initialise
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            // setup the buttons
            _view.ShowUpdateButton = true;
            _view.ShowCancelButton = true;
            _view.ShowBackButton = false;
        }
    }
}
