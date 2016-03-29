namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class SendEmailToConsultantForValuationDoneCommand : StandardDomainServiceCommand
    {
        public SendEmailToConsultantForValuationDoneCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}