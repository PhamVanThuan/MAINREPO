using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public abstract class AbstractLengthBasedVariable : AbstractCustomVariable
    {
        public AbstractLengthBasedVariable(string name, CustomVariableTypeEnum variableType, int length)
            : base(name, variableType)
        {
            this.Length = length;
        }

        public int Length { get; protected set; }

        public void UpdateLength(int length)
        {
            this.Length = length;
        }
    }
}