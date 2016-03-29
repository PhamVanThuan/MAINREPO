using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Security;

using SAHL.Common.CacheData;

namespace SAHL.Common.UI
{
    public class CBOWorkflowNode : CBOMenuNode
    {
        protected EventList<IContextMenu> _contextMenu;

        public CBOWorkflowNode(Dictionary<string, object> NodeData)
            : base(NodeData)
        {
            _isRemovable = false;
        }

        public CBOWorkflowNode(int GenericKey, CBONode Parent, string Description, string LongDescription)
            : base(null, GenericKey, Parent as CBOMenuNode, Description, LongDescription)
        {
            _isRemovable = false;
        }

        public override int GenericKeyTypeKey
        {
            get
            {
                return base._genericKeyTypeKey;
            }
        }

        public void AddStaticContextMenuNodes(List<CBONode> contextNodes)
        {
            CBOContextNode node = null;

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            // get the list of features available for the spc
            IList<int> allowedFeatures = spc.FeatureKeys;

            // Add the workflow super search contextmenu node
            node = new CBOContextNode(null, -1, null, "Workflow Search", "Workflow Search", "WorkflowSuperSearch", false);
            contextNodes.Add(node);

			// Add the workflow Archive Search contextmenu node : only if user has acess to this node (featurekey 2010)
			if (allowedFeatures.Contains(2010))
			{
				node = new CBOContextNode(null, -1, null, "Archive Search", "Archive Search", "ArchiveWorkflowSuperSearch", false);
				contextNodes.Add(node);
			}

            // Add the workflow batch reassign contextmenu node : only if user has acess to this node (featurekey 2000)
            if (allowedFeatures.Contains(2000))
            {
                node = new CBOContextNode(null, -1, null, "Batch Reassign", "Workflow Batch Reassign User", "WorkflowBatchReassign", false);
                contextNodes.Add(node);
            }

            // Add the helpdesk create request contextmenu node : only if user has acess to this node (featurekey 2005)
            if (allowedFeatures.Contains(2005))
            {
                node = new CBOContextNode(null, -1, null, "Help Desk - Create Request", "Help Desk - Create Request", "HelpDeskClientSearch", false);
                contextNodes.Add(node);
            }
        }
    }
}
