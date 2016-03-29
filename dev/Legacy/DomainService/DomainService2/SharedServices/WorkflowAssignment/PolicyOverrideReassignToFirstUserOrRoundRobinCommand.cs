namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class PolicyOverrideReassignToFirstUserOrRoundRobinCommand : StandardDomainServiceCommand
    {
        public PolicyOverrideReassignToFirstUserOrRoundRobinCommand(long instanceID, long sourceInstanceID, int genericKey, string state, SAHL.Common.Globals.Process process)
        {
            this.InstanceID = instanceID;
            this.SourceInstanceID = sourceInstanceID;
            this.GenericKey = genericKey;
            this.State = state;
            this.Process = process;
        }

        public long InstanceID { get; set; }

        public long SourceInstanceID { get; set; }

        public int GenericKey { get; set; }

        public string State { get; set; }

        public SAHL.Common.Globals.Process Process { get; set; }

        public string AssignedUserResult { get; set; }
    }
}