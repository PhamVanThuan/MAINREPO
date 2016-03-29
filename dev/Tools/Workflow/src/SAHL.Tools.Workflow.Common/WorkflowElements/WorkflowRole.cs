using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class WorkflowRole : AbstractRole
    {
        public WorkflowRole(string name, bool isDynamic, CodeSection onGetRole)
            : base(name, isDynamic, onGetRole)
        {
            this.RoleType = RoleTypeEnum.Workflow;
        }

        public Workflow ApplicableWorkflow { get; protected set; }

        public void UpdateApplicableWorkflow(Workflow applicableWorkflow)
        {
            this.ApplicableWorkflow = applicableWorkflow;
        }
    }
}