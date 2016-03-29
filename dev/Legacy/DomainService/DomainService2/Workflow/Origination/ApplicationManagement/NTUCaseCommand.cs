namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class NTUCaseCommand : StandardDomainServiceCommand
    {
        public NTUCaseCommand(int applicationkey)
        {
            this.ApplicationKey = applicationkey;
        }

        public int ApplicationKey { get; set; }

        public bool Result { get; set; }
    }
}