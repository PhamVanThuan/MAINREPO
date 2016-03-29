using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class TextVariable : AbstractLengthBasedVariable
    {
        public TextVariable(string name, int length)
            : base(name, CustomVariableTypeEnum.Text, length)
        {
        }
    }
}