using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class StringVariable : AbstractLengthBasedVariable
    {
        public StringVariable(string name, int length)
            : base(name, CustomVariableTypeEnum.String, length)
        {
        }
    }
}