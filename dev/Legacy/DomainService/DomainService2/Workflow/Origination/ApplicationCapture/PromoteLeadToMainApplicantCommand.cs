namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class PromoteLeadToMainApplicantCommand : StandardDomainServiceCommand
    {
        public int ApplicationKey { get; set; }

        public PromoteLeadToMainApplicantCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public bool Result { get; set; }
    }
}