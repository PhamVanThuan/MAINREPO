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

namespace SAHL.Web.Views.Common.Presenters
{
    public class X2InstanceRedirectSystemDecision : X2InstanceRedirectBase
    {

        public X2InstanceRedirectSystemDecision(IX2InstanceRedirect view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            // set the view message
            string message = "The workflow engine is busy processing the application but has not yet completed.";
            message += "<br/><br/>Once completed, you will automatically be redirected to the relevant screen.";
            message += "<br/><br/>Please wait....or click 'Refresh' to try again.<br/><br/>";

            _view.MessageText = message;
            _view.DisplayRefreshButton = true;

            base.OnViewInitialised(sender, e);
        }
    }
}
