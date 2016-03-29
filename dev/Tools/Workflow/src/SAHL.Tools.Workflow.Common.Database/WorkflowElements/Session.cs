namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class Session
    {
        public Session() { }

        public virtual string SessionID { get; set; }

        public virtual string Adusername { get; set; }

        public virtual System.DateTime SessionStartTime { get; set; }

        public virtual System.DateTime LastActivityTime { get; set; }
    }
}