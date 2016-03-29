namespace DomainService2.Workflow.Life
{
    public class ReadyToCallbackCommand : StandardDomainServiceCommand
    {
        public ReadyToCallbackCommand(int applicationKey, long instanceID)
        {
            this.ApplicationKey = applicationKey;
            this.InstanceID = instanceID;
        }

        public int ApplicationKey { get; protected set; }

        public long InstanceID { get; protected set; }
    }
}