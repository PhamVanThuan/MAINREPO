using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

using SAHL.Common;
using System.Collections.Generic;

namespace SAHL.Web.Views
{
    public partial class Startup : SAHL.Web.Views.Common.Blank
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //ICBOService CBOService = ServiceFactory.GetService<ICBOService>();
            CBONodeSetType nodeSet = CBOManager.GetCurrentNodeSetName(CurrentPrincipal);

            // if a CBO node is selected, transfer user to that page
            CBONode node = CBOManager.GetCurrentCBONode(CurrentPrincipal, nodeSet);
            if (node != null)
            {
                Navigator.Navigate(node.URL);
                return;
            }

            // no default node selected - navigate to the first node in the user's CBO node list
            List<CBONode> nodes = CBOManager.GetMenuNodes(CurrentPrincipal, nodeSet);
            
            if (nodes.Count > 0)
                Navigator.Navigate(nodes[0].URL);
            else
                Navigator.Navigate("Blank");

        }
    }
}
