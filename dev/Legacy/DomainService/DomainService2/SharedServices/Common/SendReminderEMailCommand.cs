namespace DomainService2.SharedServices.Common
{
    public class SendReminderEMailCommand : StandardDomainServiceCommand
    {
        public SendReminderEMailCommand(string creatorName, int applicationKey, string msg)
        {
            this.CreatorName = creatorName;
            this.ApplicationKey = applicationKey;
            this.Msg = msg;
        }

        public string CreatorName
        {
            get;
            protected set;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public string Msg
        {
            get;
            protected set;
        }
    }
}