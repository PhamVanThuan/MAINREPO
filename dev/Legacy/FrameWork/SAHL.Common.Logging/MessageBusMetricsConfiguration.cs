using System.Linq;
using System.Configuration;

namespace SAHL.Common.Logging
{
    public class MessageBusMetricsConfiguration
    {
        public string ApplicationName { get; protected set; }

        public bool PublishMetrics { get; protected set; }

        public MessageBusMetricsConfiguration()
        {
            this.ApplicationName = ConfigurationManager.AppSettings["ApplicationName"];

            if (ConfigurationManager.AppSettings.AllKeys.Contains("PublishMetrics"))
            {
                this.PublishMetrics = bool.Parse(ConfigurationManager.AppSettings["PublishMetrics"]);
            }
            else
            {
                this.PublishMetrics = false;
            }
        }
    }
}