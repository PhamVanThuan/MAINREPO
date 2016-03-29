namespace SAHL.X2Engine2.Tests.X2.Models
{
    public sealed class X2ProcessWorkflow
    {
        public int WorkflowId { get; set; }
        public string Process { get; set; }
        public string Workflow { get; set; }

        public override string ToString()
        {
            return string.Format("{0}-{1}", Process, Workflow);
        }

        public X2ProcessWorkflow(System.String Process, System.String Workflow, System.Int32 WorkflowId)
        {
            this.Process = Process;
            this.Workflow = Workflow;
            this.WorkflowId = WorkflowId;
        }
    }
}
