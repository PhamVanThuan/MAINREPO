namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class ActiveExternalActivity
    {
        public ActiveExternalActivity() { }

        public virtual int Id { get; set; }

        public virtual ExternalActivity ExternalActivity { get; set; }

        public virtual int WorkFlowID { get; set; }

        public virtual System.Nullable<long> ActivatingInstanceID { get; set; }

        public virtual System.DateTime ActivationTime { get; set; }

        public virtual string ActivityXMLData { get; set; }

        public virtual string WorkFlowProviderName { get; set; }
    }
}