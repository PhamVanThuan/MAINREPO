namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class SubmitApplicationToApplicationManagementCommand : StandardDomainServiceCommand
    {
        public long InstanceID { get; set; }

        public int ApplicationKey { get; set; }

        public SubmitApplicationToApplicationManagementCommand(long instanceID, int applicationKey)
        {
            this.InstanceID = instanceID;
            this.ApplicationKey = applicationKey;
        }
    }
}