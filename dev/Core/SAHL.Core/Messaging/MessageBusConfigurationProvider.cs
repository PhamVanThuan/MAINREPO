using SAHL.Core.Configuration;
using System.Linq;

namespace SAHL.Core.Messaging
{
    public class MessageBusConfigurationProvider : ConfigurationProvider, IMessageBusConfigurationProvider
    {
        public string HostName
        {
            get { return this.Config.AppSettings.Settings["messageBusServer"].Value; }
        }

        public string SubscriptionId
        {
            get { return this.Config.AppSettings.Settings["subscriptionId"].Value; }
        }

        public string UserName
        {
            get
            {
                string usernameConfig = "messageBusUsername";

                if (this.Config.AppSettings.Settings.AllKeys.Contains(usernameConfig))
                {
                    return this.Config.AppSettings.Settings[usernameConfig].Value;
                }

                return null;
            }
        }

        public string Password
        {
            get
            {
                string passwordConfig = "messageBusPassword";

                if (this.Config.AppSettings.Settings.AllKeys.Contains(passwordConfig))
                {
                    return this.Config.AppSettings.Settings[passwordConfig].Value;
                }

                return null;
            }
        }
    }
}