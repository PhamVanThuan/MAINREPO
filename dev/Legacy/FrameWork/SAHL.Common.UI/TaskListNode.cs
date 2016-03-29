using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Security;


namespace SAHL.Common.UI
{
    public class TaskListNode : CBOWorkflowNode
    {
        public TaskListNode(CBONode Parent)
            : base(0, Parent, "Tasks", "")
        {
            _url = "X2TaskListSummary";
            //_menuIcon = "Instance.gif";
            _menuIcon = "Instance2.png";
            _isRemovable = false;
            //base._cboUniqueKey = "TASKS";
        }

        public override void GetContextNodes(SAHLPrincipal principal, List<CBONode> contextNodes, IDictionary<int, object> allowedContextMenus)
        {
            // Add the workflow batch reassign contextmenu node
            AddStaticContextMenuNodes(contextNodes);
        }
    }
}
