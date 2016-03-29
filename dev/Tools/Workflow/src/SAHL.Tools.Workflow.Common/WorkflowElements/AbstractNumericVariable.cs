using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public abstract class AbstractNumericVariable : AbstractCustomVariable
    {
        public AbstractNumericVariable(string name, CustomVariableTypeEnum variableType)
            : base(name, variableType)
        {
        }
    }
}