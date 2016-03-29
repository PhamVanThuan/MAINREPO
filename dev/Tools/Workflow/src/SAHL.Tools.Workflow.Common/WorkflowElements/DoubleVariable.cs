using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class DoubleVariable : AbstractNumericVariable
    {
        public DoubleVariable(string name)
            : base(name, CustomVariableTypeEnum.Double)
        {
        }
    }
}