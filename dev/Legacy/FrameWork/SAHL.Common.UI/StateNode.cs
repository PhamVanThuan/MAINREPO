using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;


namespace SAHL.Common.UI
{
    public class StateNode : CBOWorkflowNode
    {

        public StateNode(int StateID, CBONode Parent, string Description, string LongDescription)
            : base(StateID, Parent, Description, LongDescription)
        {
            _url = "X2WorkList";
            //_menuIcon = "Instance.gif";
            _menuIcon = "Instance2.png";
            _isRemovable = false;
            //base._cboUniqueKey = "SN_" + StateID.ToString();
        }


    }
}
