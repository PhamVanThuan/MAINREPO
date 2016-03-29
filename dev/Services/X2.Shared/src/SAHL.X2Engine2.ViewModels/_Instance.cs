using System;

namespace SAHL.X2Engine2.ViewModels
{
    public class _Instance
    {
        public long InstanceId { get; set; }

        public int? StateID { get; set; }

        public string StateName { get; set; }

        public DateTime StateChangeDate { get; set; }

        public string WorkflowName { get; set; }

        public string ProcessName { get; set; }

        public string CreatorADUsername { get; set; }

        public _Instance()
        {
        }

        public _Instance(string processName, string workflowName, long instanceId, string stateName, int? stateId)
        {
            this.InstanceId = instanceId;
            this.WorkflowName = workflowName;
            this.ProcessName = processName;
            this.StateID = stateId;
            this.StateName = StateName;
        }
    }
}