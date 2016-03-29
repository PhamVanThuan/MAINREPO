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
using Microsoft.ApplicationBlocks.UIProcess;

namespace SAHL.Web.Controllers.Common
{
    public class CommonController : SAHLCommonBaseController
    {
        public CommonController(Navigator navigator)
            : base(navigator)
        {

        }
    }
}
