namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class ActivateNTUFromWatchdogTimeCommand : StandardDomainServiceCommand
    {
        public ActivateNTUFromWatchdogTimeCommand(long instanceID)
        {
            this.InstanceID = instanceID;
        }

        public long InstanceID { get; set; }

        public bool Result { get; set; }
    }
}