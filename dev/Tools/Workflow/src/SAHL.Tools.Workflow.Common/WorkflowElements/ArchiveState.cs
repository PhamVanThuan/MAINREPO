using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class ArchiveState : AbstractNamedState
    {
        public ArchiveState(string name, Single locationX, Single locationY, CodeSection onEnterState, CodeSection onExitState, CodeSection onReturn, Guid X2ID)
            : base(name, locationX, locationY, onEnterState, onExitState, X2ID)
        {
            this.OnReturnCode = onReturn;

            this.AddCodeSection(this.OnReturnCode);
        }

        public CodeSection OnReturnCode { get; protected set; }

        public Workflow ReturnToWorkflow { get; set; }

        public AbstractActivity ReturnActivity { get; set; }
    }
}