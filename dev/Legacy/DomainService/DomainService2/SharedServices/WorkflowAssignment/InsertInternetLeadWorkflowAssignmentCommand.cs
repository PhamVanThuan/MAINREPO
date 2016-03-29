namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class InsertInternetLeadWorkflowAssignmentCommand : StandardDomainServiceCommand
    {
        public InsertInternetLeadWorkflowAssignmentCommand(long instanceID, int applicationKey, string state)
        {
            this.ApplicationKey = applicationKey;
            this.InstanceID = instanceID;
            this.State = state;
        }

        public long InstanceID { get; set; }

        public int ApplicationKey { get; set; }

        public string State { get; set; }

        public string AssignedToResult { get; set; }
    }
}