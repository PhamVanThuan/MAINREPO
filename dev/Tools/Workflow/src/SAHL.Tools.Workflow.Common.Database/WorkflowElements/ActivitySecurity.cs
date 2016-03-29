namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class ActivitySecurity
    {
        public ActivitySecurity() { }

        public virtual int Id { get; set; }

        public virtual Activity Activity { get; set; }

        public virtual SecurityGroup SecurityGroup { get; set; }
    }
}