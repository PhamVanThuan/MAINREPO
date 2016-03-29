namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class ReturnNonDisbursedLoanToProspectCommand : StandardDomainServiceCommand
    {
        public ReturnNonDisbursedLoanToProspectCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}