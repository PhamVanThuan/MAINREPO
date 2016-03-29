namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class WorkFlowProviderInstance
    {
        public WorkFlowProviderInstance() { }

        public virtual int Id { get; set; }

        public virtual string WorkFlowProviderName { get; set; }

        public virtual System.DateTime ActiveDate { get; set; }
    }
}