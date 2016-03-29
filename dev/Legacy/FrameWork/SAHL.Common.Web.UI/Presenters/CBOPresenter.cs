using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Web.UI.Views;
using System.Security.Principal;
using SAHL.Common.Authentication;
using SAHL.Common.UI;
using SAHL.Common.Security;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.UI;

namespace SAHL.Common.Web.UI.Presenters
{
    public class CBOPresenter
    {
        #region Variable Declarations

        //ICBOService _cboService = null;
        ICBOView _cboView = null;

        #endregion

        public CBOPresenter(/*ICBOService cboService,*/ ICBOView cboView)
        {
            //_cboService = cboService;
            _cboView = cboView;

            _cboView.OnLoad += new LoadDelegate(CBOView_OnLoad);
            _cboView.OnNodeSelected += new NodeSelectedDelegate(CBOView_OnNodeSelected);
        }

        #region Event Handlers

        void CBOView_OnLoad(object sender, SAHLPrincipal Principal)
        {
            //IDomainMessageCollection mc = new DomainMessageCollection();
            IEventList<CBONode> Nodes = CBOManager.GetMenuNodes(Principal, CBONodeSetType.CBO);
            _cboView.RenderNodes(Nodes);
        }

        void CBOView_OnNodeSelected(object sender, SAHLPrincipal Principal, CBONode SelectedNode)
        {
            CBOManager.SetCurrentCBONode(Principal, SelectedNode as CBOMenuNode, CBONodeSetType.CBO); 
        }

        #endregion
    }
}
