using System;
using System.Configuration;

namespace SAHL.Communication
{
    public class EasyNetQMessageBusConfigurationProvider : MarshalByRefObject, IMessageBusConfigurationProvider
    {
        public EasyNetQMessageBusConfigurationProvider()
        {
            ApplicationName = ConfigurationManager.AppSettings["ApplicationName"];
            MessageServer = ConfigurationManager.AppSettings["MessageServer"];
            UserName = ConfigurationManager.AppSettings["messageBusUsername"];
            Password = ConfigurationManager.AppSettings["messageBusPassword"];
            QueueName = string.Format("{0}_{1}", Environment.MachineName, ApplicationName);
        }

        public string ApplicationName
        {
            get;
            set;
        }

        public string MessageServer
        {
            get;
            set;
        }

        public string QueueName
        {
            get;
            protected set;
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