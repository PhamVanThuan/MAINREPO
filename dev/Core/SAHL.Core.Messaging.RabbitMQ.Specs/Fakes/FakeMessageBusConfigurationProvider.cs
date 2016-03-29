namespace SAHL.Core.Messaging.RabbitMQ.Specs.Fakes
{
    public class FakeMessageBusConfigurationProvider : IMessageBusConfigurationProvider
    {
        public FakeMessageBusConfigurationProvider(string hostName, string userName, string password)
        {
            this.HostName = hostName;
            this.UserName = userName;
            this.Password = password;
        }

        public System.Configuration.Configuration Config
        {
            get
            {
                return null;
            }
        }

        public string HostName
        {
            get; set;
        }

        public string Password
        {
            get; set;
        }

        public string SubscriptionId
        {
            get; set;
        }

        public string UserName
        {
            get; set;
        }
    }
}