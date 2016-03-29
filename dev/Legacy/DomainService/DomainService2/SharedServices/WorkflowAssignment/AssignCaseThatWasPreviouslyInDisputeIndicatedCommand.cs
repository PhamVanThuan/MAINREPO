namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class AssignCaseThatWasPreviouslyInDisputeIndicatedCommand : StandardDomainServiceCommand
    {
        public AssignCaseThatWasPreviouslyInDisputeIndicatedCommand(int applicationKey, long instanceID)
        {
            this.ApplicationKey = applicationKey;
            this.InstanceID = instanceID;
        }

        public int ApplicationKey { get; set; }

        public long InstanceID { get; set; }

        public string AssignedToResult { get; set; }
    }
}