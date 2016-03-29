namespace DomainService2.Workflow.Origination.Valuations
{
    public class CheckIfCanPerformFurtherValuationCommand : StandardDomainServiceCommand
    {
        public CheckIfCanPerformFurtherValuationCommand(long instanceID)
        {
            this.InstanceID = instanceID;
        }

        public long InstanceID { get; set; }

        public bool Result { get; set; }
    }
}