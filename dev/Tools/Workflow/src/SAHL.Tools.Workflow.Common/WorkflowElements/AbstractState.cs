using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class AbstractState : AbstractPositionableElement
    {
        public AbstractState(Single locationX, Single locationY)
            : base(locationX, locationY)
        {
        }
    }
}