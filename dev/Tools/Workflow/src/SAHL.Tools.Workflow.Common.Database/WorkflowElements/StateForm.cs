namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class StateForm
    {
        public StateForm() { }

        public virtual int Id { get; set; }

        public virtual State State { get; set; }

        public virtual Form Form { get; set; }

        public virtual int FormOrder { get; set; }
    }
}