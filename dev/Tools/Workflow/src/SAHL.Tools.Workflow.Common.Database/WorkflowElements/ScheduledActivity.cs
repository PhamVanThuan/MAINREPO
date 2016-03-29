namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class ScheduledActivity
    {
        public ScheduledActivity() { }

        public virtual Instance Instance { get; set; }

        public virtual System.DateTime Time { get; set; }

        public virtual Activity Activity { get; set; }

        public virtual int Priority { get; set; }

        public virtual string WorkFlowProviderName { get; set; }

        public virtual int Id { get; set; }
    }
}