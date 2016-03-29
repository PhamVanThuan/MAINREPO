namespace DomainService2.Workflow.Origination.Credit
{
    public class IsValuationApprovalRequiredCommand : StandardDomainServiceCommand
    {
        public IsValuationApprovalRequiredCommand(long instanceID)
        {
            this.InstanceID = instanceID;
        }

        public long InstanceID { get; set; }

        public bool Result { get; set; }
    }
}