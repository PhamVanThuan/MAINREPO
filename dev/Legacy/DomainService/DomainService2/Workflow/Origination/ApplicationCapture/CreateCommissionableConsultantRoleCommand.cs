namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class CreateCommissionableConsultantRoleCommand : StandardDomainServiceCommand
    {
        public int ApplicationKey { get; set; }

        public string ADUserName { get; set; }

        public CreateCommissionableConsultantRoleCommand(int applicationKey, string adUserName)
        {
            this.ApplicationKey = applicationKey;
            this.ADUserName = adUserName;
        }
    }
}