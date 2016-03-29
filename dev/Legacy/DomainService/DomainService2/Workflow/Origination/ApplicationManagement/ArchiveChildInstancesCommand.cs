namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class ArchiveChildInstancesCommand : StandardDomainServiceCommand
    {
        public ArchiveChildInstancesCommand(long instanceID, string adUser)
        {
            this.InstanceID = instanceID;
            this.ADUser = adUser;
        }

        public long InstanceID { get; set; }

        public string ADUser { get; set; }

        public bool Result { get; set; }
    }
}