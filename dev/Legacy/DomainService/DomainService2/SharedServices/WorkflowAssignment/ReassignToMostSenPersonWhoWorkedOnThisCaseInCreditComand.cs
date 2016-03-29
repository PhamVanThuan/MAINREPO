namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReassignToMostSenPersonWhoWorkedOnThisCaseInCreditCommand : StandardDomainServiceCommand
    {
        public ReassignToMostSenPersonWhoWorkedOnThisCaseInCreditCommand(long instanceID, long sourceInstanceID, int applicationKey, string state)
        {
            this.InstanceID = instanceID;
            this.SourceInstanceID = sourceInstanceID;
            this.ApplicationKey = applicationKey;
            this.State = state;
        }

        public long InstanceID { get; set; }

        public long SourceInstanceID { get; set; }

        public int ApplicationKey { get; set; }

        public string State { get; set; }

        public string AssignedUserResult { get; set; }
    }
}