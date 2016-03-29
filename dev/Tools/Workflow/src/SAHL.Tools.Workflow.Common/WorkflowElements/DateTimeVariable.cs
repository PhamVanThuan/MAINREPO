using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class DateTimeVariable : AbstractCustomVariable
    {
        public DateTimeVariable(string name)
            : base(name, CustomVariableTypeEnum.DateTime)
        {
        }
    }
}