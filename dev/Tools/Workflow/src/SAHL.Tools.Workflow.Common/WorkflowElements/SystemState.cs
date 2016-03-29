using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class SystemState : AbstractNamedState
    {
        public SystemState(string name, Single locationX, Single locationY, bool useAutoForward, CodeSection onEnterState, CodeSection onExitState, CodeSection autoForwardCode, Guid X2ID)
            : base(name, locationX, locationY, onEnterState, onExitState, X2ID)
        {
            this.UseAutoForward = useAutoForward;
            this.AutoForwardCode = autoForwardCode;

            this.AddCodeSection(this.AutoForwardCode);
        }

        public bool UseAutoForward { get; protected set; }

        public void UpdateUseAutoForward(bool useAutoForward)
        {
            this.UseAutoForward = useAutoForward;
        }

        public CodeSection AutoForwardCode { get; protected set; }
    }
}