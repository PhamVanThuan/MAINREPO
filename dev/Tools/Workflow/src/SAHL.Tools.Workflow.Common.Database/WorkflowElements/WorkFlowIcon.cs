using System.Collections.Generic;

namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class WorkFlowIcon
    {
        public WorkFlowIcon()
        {
        }

        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual byte[] Icon { get; set; }
    }
}