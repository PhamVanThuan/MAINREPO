namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReturnPolicyOverrideUserCommand : StandardDomainServiceCommand
    {
        public ReturnPolicyOverrideUserCommand(long instanceID)
        {
            this.InstanceID = instanceID;
        }

        public long InstanceID { get; set; }

        public string UserResult { get; set; }
    }
}