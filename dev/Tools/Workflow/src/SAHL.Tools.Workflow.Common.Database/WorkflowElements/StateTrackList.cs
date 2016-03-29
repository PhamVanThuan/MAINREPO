namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class StateTrackList
    {
        public StateTrackList() { }

        public virtual int Id { get; set; }

        public virtual State State { get; set; }

        public virtual SecurityGroup SecurityGroup { get; set; }
    }
}