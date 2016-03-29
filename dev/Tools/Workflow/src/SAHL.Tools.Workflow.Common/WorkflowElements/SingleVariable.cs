using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class SingleVariable : AbstractNumericVariable
    {
        public SingleVariable(string name)
            : base(name, CustomVariableTypeEnum.Single)
        {
        }
    }
}