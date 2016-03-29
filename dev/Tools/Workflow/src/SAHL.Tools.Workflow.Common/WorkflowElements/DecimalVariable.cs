using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class DecimalVariable : AbstractNumericVariable
    {
        public DecimalVariable(string name)
            : base(name, CustomVariableTypeEnum.Decimal)
        {
        }
    }
}