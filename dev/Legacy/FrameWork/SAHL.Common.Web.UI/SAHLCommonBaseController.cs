using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.Web.UI
{
    public class SAHLCommonBaseController : ControllerBase 
    {
        public SAHLCommonBaseController(INavigator navigator)
            : base(navigator)
        {

        }
    }
}
