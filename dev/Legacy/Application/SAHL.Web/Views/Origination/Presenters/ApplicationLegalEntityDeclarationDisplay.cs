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

using SAHL.Common.UI;

namespace SAHL.Web.Views.Origination.Presenters
{
    public class ApplicationLegalEntityDeclarationDisplay : ApplicationLegalEntityDeclarationBase
    {
        private CBOMenuNode _node;

        public ApplicationLegalEntityDeclarationDisplay(IApplicationLegalEntityDeclaration view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            // get the legalentitykey and the applicationkey from the cbo
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node != null)
            {
                base.LegalEntityKey = _node.GenericKey;
                base.ApplicationKey = _node.ParentNode.GenericKey;
            }

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
            _view.ShowUpdateButton = false;
            _view.ShowCancelButton = false;
            _view.ShowBackButton = false;           
        }
    }
}
