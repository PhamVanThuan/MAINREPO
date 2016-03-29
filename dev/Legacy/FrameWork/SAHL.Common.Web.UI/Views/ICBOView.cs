using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using SAHL.Common.Authentication;
using SAHL.Common.UI;
using SAHL.Common.Security;
using SAHL.Common.Collections.Interfaces;


namespace SAHL.Common.Web.UI.Views
{
    #region Delegates

    /// <summary>
    /// Delegate for a node being selected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="principal"></param>
    /// <param name="SelectedNode"></param>
    public delegate void NodeSelectedDelegate(object sender, SAHLPrincipal principal, CBONode SelectedNode);
    public delegate void LoadDelegate(object sender, SAHLPrincipal principal);

    #endregion

    #region CBO View Interface definition

    public interface ICBOView
    {
        event NodeSelectedDelegate OnNodeSelected;
        event LoadDelegate OnLoad;
        void RenderNodes(List<CBONode> Nodes);
    }

    #endregion 
}
