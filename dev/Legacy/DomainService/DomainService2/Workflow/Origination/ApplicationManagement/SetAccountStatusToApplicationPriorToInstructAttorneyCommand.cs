namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class SetAccountStatusToApplicationPriorToInstructAttorneyCommand : StandardDomainServiceCommand
    {
        public SetAccountStatusToApplicationPriorToInstructAttorneyCommand(int applicationKey, string adusername)
        {
            this.ApplicationKey = applicationKey;
            this.ADUserName = adusername;
        }

        public int ApplicationKey { get; set; }

        public string ADUserName { get; set; }
    }
}