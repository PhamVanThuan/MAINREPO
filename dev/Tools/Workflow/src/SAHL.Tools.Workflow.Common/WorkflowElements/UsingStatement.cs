using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class UsingStatement : AbstractNamedElement
    {
        public UsingStatement(string name)
            : base(name)
        {
        }
    }
}