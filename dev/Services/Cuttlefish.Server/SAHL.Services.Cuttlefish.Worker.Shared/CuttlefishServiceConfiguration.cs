using SAHL.Core.Configuration;
using System;

namespace SAHL.Services.Cuttlefish.Worker.Shared
{
    public class CuttlefishServiceConfiguration : ConfigurationProvider, ICuttlefishServiceConfiguration
    {
        public int WorkerCountForv2LogMessage
        {
            get { return Convert.ToInt32(this.Config.AppSettings.Settings["Worker.Count.v2LogMessage"].Value); }
        }

        public int WorkerCountForv3LogMessage
        {
            get { return Convert.ToInt32(this.Config.AppSettings.Settings["Worker.Count.v3LogMessage"].Value); }
        }

        public int WorkerCountForv2ThroughputMessage
        {
            get { return Convert.ToInt32(this.Config.AppSettings.Settings["Worker.Count.v2ThroughputMessage"].Value); }
        }

        public int WorkerCountForv3ThroughputMessage
        {
            get { return Convert.ToInt32(this.Config.AppSettings.Settings["Worker.Count.v3ThroughputMessage"].Value); }
        }

        public int WorkerCountForv2LatencyMessage
        {
            get { return Convert.ToInt32(this.Config.AppSettings.Settings["Worker.Count.v2LatencyMessage"].Value); }
        }

        public int WorkerCountForv3LatencyMessage
        {
            get { return Convert.ToInt32(this.Config.AppSettings.Settings["Worker.Count.v3LatencyMessage"].Value); }
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


        public string ConnectionString
        {
            get { return this.Config.ConnectionStrings.ConnectionStrings["DBCONNECTION_ServiceArchitect"].ConnectionString; }
        }
    }
}