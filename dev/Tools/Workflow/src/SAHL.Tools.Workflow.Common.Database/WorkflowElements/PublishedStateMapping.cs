namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class PublishedStateMapping
    {
        public PublishedStateMapping() { }

        public virtual int OldWorkFlowID { get; set; }

        public virtual int OldStateID { get; set; }

        public virtual int NewWorkFlowID { get; set; }

        public virtual int NewStateID { get; set; }

        public override bool Equals(object obj)
        {
            if (obj as PublishedStateMapping == null)
            {
                return false;
            }

            PublishedStateMapping objP = obj as PublishedStateMapping;

            if ((objP.NewStateID == this.NewStateID) &&
                (objP.NewWorkFlowID == this.NewWorkFlowID) &&
                (objP.OldStateID == this.NewStateID) &&
                (objP.OldWorkFlowID == this.OldWorkFlowID))
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}