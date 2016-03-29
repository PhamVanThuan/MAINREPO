using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

using SAHL.Common;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Security;
using SAHL.Common.Attributes;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;




namespace SAHL.Common.UI
{
    public class WorkFlowNode : CBOWorkflowNode
    {

        public WorkFlowNode(int WorkFlowID, CBONode Parent, string Description, string LongDescription)
            : base(WorkFlowID, Parent, Description, LongDescription)
        {
            _url = "X2WorkFlowSummary";
            _isRemovable = false;

            switch (Description)
            {
                case "LifeOrigination" :
                    _menuIcon = "IconHeart.gif";
                    break;

                case "Personal Loans":
                    _menuIcon = "PersonalLoan.gif";
                    break;

                default:
                    _menuIcon = "Workflow.gif";
                    break;

            }
            //base._cboUniqueKey = "WN_" + WorkFlowID.ToString();
        }

        public override void GetContextNodes(SAHLPrincipal principal, List<CBONode> contextNodes, IDictionary<int, object> allowedContextMenus)
        {
            // Add the workflow batch reassign contextmenu node
            AddStaticContextMenuNodes(contextNodes);
        }
    }
}
