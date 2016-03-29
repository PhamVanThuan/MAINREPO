using System;
using System.Configuration;

namespace SAHL.Common.Logging
{
    public class MessageBusLoggerConfiguration
    {
        public string ApplicationName { get; protected set; }

        public int LogLevel { get; protected set; }

        public MessageBusLoggerConfiguration()
        {
            ApplicationName = ConfigurationManager.AppSettings["ApplicationName"];
            LogLevel = String.IsNullOrEmpty(ConfigurationManager.AppSettings["LogLevel"]) ? 1 : Convert.ToInt32(ConfigurationManager.AppSettings["LogLevel"].ToString());
        }
    }
}