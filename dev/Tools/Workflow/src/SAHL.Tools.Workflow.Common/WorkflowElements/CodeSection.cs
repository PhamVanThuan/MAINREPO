using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class CodeSection : AbstractNamedElement
    {
        public CodeSection(string name)
            : this(name, "")
        {
        }

        public CodeSection(string name, string code)
            : base(name)
        {
            this.Code = code;
        }

        public string Code { get; protected set; }

        public void UpdateCode(string code)
        {
            this.Code = code;
        }
    }
}