namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class CreateInternetApplicationCommand : StandardDomainServiceCommand
    {
        public int ApplicationKey { get; protected set; }
        public string ApplicationData { get; protected set; }

        public CreateInternetApplicationCommand(int applicationKey, string applicationData)
        {
            this.ApplicationKey = applicationKey;
            this.ApplicationData = applicationData;
        }

        public bool Result { get; set; }
    }
}
