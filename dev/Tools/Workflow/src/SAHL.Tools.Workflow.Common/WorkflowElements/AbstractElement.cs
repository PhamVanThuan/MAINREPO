using System;
using System.Collections.Generic;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public abstract class AbstractElement
    {
        private List<CodeSection> allCodeSections;

        public AbstractElement()
        {
            this.allCodeSections = new List<CodeSection>();
        }

        public CodeSection[] AllCodeSections
        {
            get
            {
                return this.allCodeSections.ToArray();
            }
        }

        protected void AddCodeSection(CodeSection codeSection)
        {
            if (codeSection != null && !this.allCodeSections.Contains(codeSection))
            {
                this.allCodeSections.Add(codeSection);
            }
        }
    }
}