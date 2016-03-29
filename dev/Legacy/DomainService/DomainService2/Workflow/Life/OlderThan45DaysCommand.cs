namespace DomainService2.Workflow.Life
{
    public class OlderThan45DaysCommand : StandardDomainServiceCommand
    {
        public OlderThan45DaysCommand(int applicationKey, long instanceID)
        {
            this.ApplicationKey = applicationKey;
            this.InstanceID = instanceID;
        }

        public int ApplicationKey { get; protected set; }

        public long InstanceID { get; protected set; }
    }
}