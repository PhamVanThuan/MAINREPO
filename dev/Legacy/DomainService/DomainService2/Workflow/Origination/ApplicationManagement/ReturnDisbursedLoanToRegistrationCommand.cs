namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class ReturnDisbursedLoanToRegistrationCommand : StandardDomainServiceCommand
    {
        public ReturnDisbursedLoanToRegistrationCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}