using SAHL.Core.Configuration;
using System;

namespace SAHL.Services.Cuttlefish.Services
{
    public class CuttlefishServiceConfigurationProvider : ConfigurationProvider, ICuttlefishServiceConfigurationProvider
    {
        public int WorkerCountForv2LogMessage
        {
            get { return Convert.ToInt32(this.Config.AppSettings.Settings["Worker.Count.v2LogMessage"].Value); }
        }

        public int WorkerCountForv2ThroughputMessage
        {
            get { return Convert.ToInt32(this.Config.AppSettings.Settings["Worker.Count.v2ThroughputMessage"].Value); }
        }

        public int WorkerCountForv2LatencyMessage
        {
            get { return Convert.ToInt32(this.Config.AppSettings.Settings["Worker.Count.v2LatencyMessage"].Value); }
        }

        public string MessageBusServer
        {
            get { return this.Config.AppSettings.Settings["messageBusServer"].Value; }
        }

        public string MessageBusServerUserName
        {
            get { return this.Config.AppSettings.Settings["messageBusUsername"].Value; }
        }

        public string MessageBusServerPassword
        {
            get { return this.Config.AppSettings.Settings["messageBusPassword"].Value; }
        }
    }
}