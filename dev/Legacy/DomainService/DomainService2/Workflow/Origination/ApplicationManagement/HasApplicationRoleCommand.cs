namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class HasApplicationRoleCommand : StandardDomainServiceCommand
    {
        public HasApplicationRoleCommand(int applicationKey, int applicationRoleTypeKey)
        {
            this.ApplicationKey = applicationKey;
            this.ApplicationRoleTypeKey = applicationRoleTypeKey;
        }

        public int ApplicationKey { get; set; }

        public int ApplicationRoleTypeKey { get; set; }

        public bool Result { get; set; }
    }
}