namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class SendAlphaHousingSurveyEmailCommand : StandardDomainServiceCommand
    {
        public int ApplicationKey { get; set; }

        public bool AlphaHousingEmailSent { get; set; }

        public SendAlphaHousingSurveyEmailCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
            this.AlphaHousingEmailSent = false;
        }
    }
}