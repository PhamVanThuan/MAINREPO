using System;
using System.Configuration;
using System.Linq;

namespace SAHL.Communication
{
    public class X2EasyNetQMessageBusConfigurationProvider : MarshalByRefObject, IMessageBusConfigurationProvider
    {
        public X2EasyNetQMessageBusConfigurationProvider()
        {
            ApplicationName = ConfigurationManager.AppSettings["ApplicationName"];
            MessageServer = ConfigurationManager.AppSettings["MessageServer"];

            string usernameConfig = "messageBusUsername";
            string passwordConfig = "messageBusPassword";
            if (ConfigurationManager.AppSettings.AllKeys.Contains(usernameConfig))
            {
                UserName = ConfigurationManager.AppSettings[usernameConfig];
            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains(passwordConfig))
            {
                Password = ConfigurationManager.AppSettings[passwordConfig];
            }
        }

        public string ApplicationName
        {
            get;
            protected set;
        }

        public string MessageServer
        {
            get;
            protected set;
        }

        public string X2ProcessName
        {
            get;
            set;
        }

        public string QueueName
        {
            get
            {
                if (string.IsNullOrEmpty(X2ProcessName))
                {
                    X2ProcessName = Guid.NewGuid().ToString();
                }

                return string.Format("{0}_{1}_{2}", Environment.MachineName, ApplicationName, X2ProcessName);
            }
        }

        public string UserName
        {
            get;
            protected set;
        }

        public string Password
        {
            get;
            protected set;
        }
    }
}