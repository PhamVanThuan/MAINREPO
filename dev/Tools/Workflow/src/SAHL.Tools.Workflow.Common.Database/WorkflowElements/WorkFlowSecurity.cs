namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class WorkFlowSecurity
    {
        public WorkFlowSecurity() { }

        public virtual int Id { get; set; }

        public virtual WorkFlow WorkFlow { get; set; }

        public virtual SecurityGroup SecurityGroup { get; set; }
    }
}