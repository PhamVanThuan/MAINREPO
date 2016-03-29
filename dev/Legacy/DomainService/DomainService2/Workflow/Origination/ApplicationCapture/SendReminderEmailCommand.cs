namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class SendReminderEmailCommand : StandardDomainServiceCommand
    {
        public int ApplicationKey { get; set; }

        public long InstanceID { get; set; }

        public SendReminderEmailCommand(int applicationKey, long instanceID)
        {
            this.ApplicationKey = applicationKey;
            this.InstanceID = instanceID;
        }
    }
}