namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class SaveApplicationCommand : StandardDomainServiceCommand
    {
        public SaveApplicationCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }

        public bool Result { get; set; }
    }
}