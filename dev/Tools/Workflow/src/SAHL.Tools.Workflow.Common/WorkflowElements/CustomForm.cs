using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class CustomForm : AbstractNamedElement
    {
        public CustomForm(string name, string description)
            : base(name)
        {
            this.Description = description;
        }

        public string Description { get; protected set; }

        public void UpdateDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException("Description may not be empty.", "name");
            }

            this.Description = description;
        }
    }
}