using System.Collections.Generic;

namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class Form
    {
        public Form()
        {
        }

        public virtual int Id { get; set; }

        public virtual WorkFlow WorkFlow { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }
    }
}