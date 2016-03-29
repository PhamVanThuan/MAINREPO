namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class WorkList
    {
        public WorkList() { }

        public virtual Instance Instance { get; set; }

        public virtual int Id { get; set; }

        public virtual string Adusername { get; set; }

        public virtual System.DateTime ListDate { get; set; }

        public virtual string Message { get; set; }
    }
}