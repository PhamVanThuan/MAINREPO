namespace DomainService2.Workflow.HelpDesk
{
    public class CreateRequestCommand : StandardDomainServiceCommand
    {
        public CreateRequestCommand(int legalEntityKey)
        {
            this.LegalEntityKey = legalEntityKey;
        }

        public int LegalEntityKey
        {
            get;
            protected set;
        }

        public string Result
        {
            get;
            set;
        }
    }
}