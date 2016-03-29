using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class BoolVariable : AbstractCustomVariable
    {
        public BoolVariable(string name)
            : base(name, CustomVariableTypeEnum.Bool)
        {
        }
    }
}