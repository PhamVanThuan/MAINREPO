using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class CommonState : AbstractNamedState
    {
        private List<AbstractNamedState> appliesTo;

        public CommonState(string name, Single locationX, Single locationY, CodeSection onEnterState, CodeSection onExitState,Guid X2ID)
            : base(name, locationX, locationY, onEnterState, onExitState, X2ID)
        {
            this.appliesTo = new List<AbstractNamedState>();
        }

        public ReadOnlyCollection<AbstractNamedState> AppliesTo
        {
            get
            {
                return new ReadOnlyCollection<AbstractNamedState>(this.appliesTo);
            }
        }

        public void ApplyToState(AbstractNamedState stateToApplyTo)
        {
            if (stateToApplyTo is CommonState)
            {
            }

            if (!this.appliesTo.Contains(stateToApplyTo))
            {
                this.appliesTo.Add(stateToApplyTo);
            }
        }

        public void RemoveFromState(AbstractNamedState stateToRemoveFrom)
        {
            if (this.appliesTo.Contains(stateToRemoveFrom))
            {
                this.appliesTo.Remove(stateToRemoveFrom);
            }
        }
    }
}