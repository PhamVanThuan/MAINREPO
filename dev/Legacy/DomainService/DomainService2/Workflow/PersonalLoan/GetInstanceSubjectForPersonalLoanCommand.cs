namespace DomainService2.Workflow.PersonalLoan
{
    public class GetInstanceSubjectForPersonalLoanCommand : StandardDomainServiceCommand
    {
        public int ApplicationKey { get; protected set; }

        public string Result { get; set; }

        public GetInstanceSubjectForPersonalLoanCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }
    }
}