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
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Life.Interfaces
{
    public interface ICallBackHold : IViewBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstCallbacks"></param>
        void BindCallBackGrid(IEventList<ICallback> lstCallbacks);

    }
}
