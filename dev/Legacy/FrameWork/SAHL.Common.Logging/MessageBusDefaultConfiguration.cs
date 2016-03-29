using System;
using System.Configuration;
using System.Linq;

namespace SAHL.Common.Logging
{
    public class MessageBusDefaultConfiguration
    {
        public string ApplicationName { get; protected set; }

        public MessageBusDefaultConfiguration()
        {
            ApplicationName = ConfigurationManager.AppSettings["ApplicationName"];
        }
    }
}