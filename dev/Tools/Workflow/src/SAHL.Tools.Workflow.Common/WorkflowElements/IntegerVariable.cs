using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class IntegerVariable : AbstractNumericVariable
    {
        public IntegerVariable(string name)
            : base(name, CustomVariableTypeEnum.Integer)
        {
        }
    }
}