using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public abstract class AbstractCustomVariable : AbstractNamedElement
    {
        public AbstractCustomVariable(string name, CustomVariableTypeEnum variableType)
            : base(name)
        {
            this.VariableType = variableType;
        }

        public CustomVariableTypeEnum VariableType { get; protected set; }
    }
}