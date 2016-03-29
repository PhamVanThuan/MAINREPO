namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class TrackList
    {
        public TrackList() { }

        public virtual int Id { get; set; }

        public virtual Instance Instance { get; set; }

        public virtual string Adusername { get; set; }

        public virtual System.DateTime ListDate { get; set; }
    }
}