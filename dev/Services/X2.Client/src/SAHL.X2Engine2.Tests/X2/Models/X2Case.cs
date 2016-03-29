using System;
namespace SAHL.X2Engine2.Tests.X2.Models
{
    public sealed class X2Case
    {
        public string Process { get; set; }
        public long InstanceId { get; set; }
        public string Workflow { get; set; }
        public string State { get; set; }

        public X2Case(System.Int64 InstanceId, System.String State, System.String Workflow, System.String Process)
        {
            this.State = State;
            this.Workflow = Workflow;
            this.InstanceId = InstanceId;
            this.Process = Process;
        }
    }
}
