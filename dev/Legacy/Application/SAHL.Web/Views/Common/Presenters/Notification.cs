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
    public class Notification : SAHLCommonBasePresenter<INotification>
    {

        public Notification(INotification view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
    
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (base.GlobalCacheData.ContainsKey("NotificationMessage"))
            {
                _view.NotificationText = (string)base.GlobalCacheData["NotificationMessage"];
            }
            
        }

    }
}
