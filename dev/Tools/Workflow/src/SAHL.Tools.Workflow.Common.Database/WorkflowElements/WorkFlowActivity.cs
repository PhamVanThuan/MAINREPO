namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class WorkFlowActivity
    {
        public WorkFlowActivity() { }

        public virtual int Id { get; set; }

        public virtual WorkFlow WorkFlow { get; set; }

        public virtual WorkFlow NextWorkFlow { get; set; }

        public virtual Activity NextActivity { get; set; }

        public virtual string Name { get; set; }

        public virtual State State { get; set; }

        public virtual Activity ReturnActivity { get; set; }
    }
}