namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class InstanceActivitySecurity
    {
        public InstanceActivitySecurity() { }

        public virtual Instance Instance { get; set; }

        public virtual Activity Activity { get; set; }

        public virtual int Id { get; set; }

        public virtual string Adusername { get; set; }
    }
}