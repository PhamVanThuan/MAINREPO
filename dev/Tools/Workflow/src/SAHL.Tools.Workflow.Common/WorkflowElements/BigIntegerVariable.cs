using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class BigIntegerVariable : AbstractNumericVariable
    {
        public BigIntegerVariable(string name)
            : base(name, CustomVariableTypeEnum.BigInteger)
        {
        }
    }
}