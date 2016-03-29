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
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CallBackConsidering : CallBack
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CallBackConsidering(SAHL.Web.Views.Life.Interfaces.ICallBack view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            // Set the callback reason to be 'Client Considering'
            CallbackReasonDescription = "Client Considering";

            base.OnViewInitialised(sender, e);
        }
    }
}
