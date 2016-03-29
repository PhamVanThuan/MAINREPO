using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class HoldState : AbstractNamedState
    {
        public HoldState(string name, Single locationX, Single locationY, CodeSection onEnterState, CodeSection onExitState,Guid X2ID)
            : base(name, locationX, locationY, onEnterState, onExitState, X2ID)
        {
        }
    }
}