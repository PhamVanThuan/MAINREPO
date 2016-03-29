using System;
using System.Data;
using System.Text;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common;
using SAHL.Common.Web.UI;
using SAHL.Common.Authentication;

using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Service;
using System.Collections.Generic;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class X2NonUserState : SAHLCommonBaseView, IX2NonUserState
    {

        #region IX2NonUserState Members

        public void SetText(string text, bool showRefreshButton)
        {
            this.Label1.Text = text;
            this.btnRefresh.Visible = showRefreshButton;
        }

        #endregion
    }
}
