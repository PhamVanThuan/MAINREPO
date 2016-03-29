namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class DemoteMainApplicantToLeadCommand : StandardDomainServiceCommand
    {
        public int ApplicationKey { get; set; }

        public DemoteMainApplicantToLeadCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public bool Result { get; set; }
    }
}