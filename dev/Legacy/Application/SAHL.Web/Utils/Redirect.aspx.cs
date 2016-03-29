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
using SAHL.Common.Security;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;

using SAHL.Common;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Utils
{
    /// <summary>
    /// Standard web page that is used for redirecting.  This can be passed a querystring ("view") and it 
    /// will restart the UIP process, with the end result being navigation to the view supplied.  This 
    /// will also (optionally) take a node set name and set that for the user - querystring key = "nodeset".  
    /// The page uses the CBOManager, so the target view MUST be in the user's set of Menu nodes for the 
    /// current node set or the supplied node set.
    /// </summary>
    public partial class Redirect : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //ICBOService CBOService = ServiceFactory.GetService<ICBOService>();
            CBOManager CBOManager = new CBOManager();
            //IViewBase view = this.Page as IViewBase;
            SAHLPrincipal principal = SAHLPrincipal.GetCurrent();
            string targetView = Request.QueryString["view"];
            string nodeSetStr = Request.QueryString["nodeset"];

            if (nodeSetStr != null)
                nodeSetStr = nodeSetStr.ToUpper();

            CBONodeSetType nodeSet;

             // set the node set if it has been supplied
            if (!String.IsNullOrEmpty(nodeSetStr))
            {
                nodeSet = (CBONodeSetType)Enum.Parse(typeof(CBONodeSetType), nodeSetStr, true);
                CBOManager.SetCurrentNodeSet(principal, nodeSet);
            }
            else
            {
                nodeSet = CBOManager.GetCurrentNodeSetName(principal);
            }

            // find the node with the view name in the CBO
            if (!String.IsNullOrEmpty(targetView))
            {
                CBOMenuNode menuNode = CBOManager.GetCBOMenuNodeByUrl(principal, targetView, nodeSet);
                if (menuNode == null)
                    throw new NullReferenceException(String.Format("Unable to find node {0}", targetView));
                CBOManager.SetCurrentCBONode(principal, menuNode, nodeSet);
            }

            UIPManager.StartNavigationTask("SAHL");
        }
    }
}
