namespace DomainService2.SharedServices.Common
{
    public class SendSMSToMainApplicantsCommand : StandardDomainServiceCommand
    {
        public SendSMSToMainApplicantsCommand(string message, int applicationKey)
        {
            this.Message = message;
            this.ApplicationKey = applicationKey;
        }

        public string Message
        {
            get;
            protected set;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }
    }
}