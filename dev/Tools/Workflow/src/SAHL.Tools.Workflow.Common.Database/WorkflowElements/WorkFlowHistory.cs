namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class WorkFlowHistory
    {
        public WorkFlowHistory() { }

        public virtual int Id { get; set; }

        public virtual State State { get; set; }

        public virtual Activity Activity { get; set; }

        public virtual long InstanceID { get; set; }

        public virtual string CreatorADUserName { get; set; }

        public virtual System.DateTime CreationDate { get; set; }

        public virtual System.DateTime StateChangeDate { get; set; }

        public virtual System.Nullable<System.DateTime> DeadlineDate { get; set; }

        public virtual System.DateTime ActivityDate { get; set; }

        public virtual string Adusername { get; set; }

        public virtual System.Nullable<int> Priority { get; set; }
    }
}