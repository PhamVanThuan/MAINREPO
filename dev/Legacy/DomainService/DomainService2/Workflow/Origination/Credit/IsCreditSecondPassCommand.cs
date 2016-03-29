namespace DomainService2.Workflow.Origination.Credit
{
    public class IsCreditSecondPassCommand : StandardDomainServiceCommand
    {
        public IsCreditSecondPassCommand(long instanceID)
        {
            this.InstanceID = instanceID;
        }

        public long InstanceID { get; set; }

        public bool Result { get; set; }
    }
}