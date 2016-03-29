using SAHL.Services.Cuttlefish.Services;

namespace SAHL.Services.Cuttlefish.Specs.Fakes
{
    public class FakeCuttlefishServiceConfiguration : ICuttlefishServiceConfigurationProvider
    {
        public string MessageBusServer
        {
            get;
            set;
        }

        public string MessageBusServerPassword
        {
            get;
            set;
        }

        public string MessageBusServerUserName
        {
            get;
            set;
        }

        public int WorkerCountForv2LatencyMessage
        {
            get;
            set;
        }

        public int WorkerCountForv2LogMessage
        {
            get;
            set;
        }

        public int WorkerCountForv2ThroughputMessage
        {
            get;
            set;
        }

        public int WorkerCountForv3LatencyMessage
        {
            get;
            set;
        }

        public int WorkerCountForv3LogMessage
        {
            get;
            set;
        }

        public int WorkerCountForv3ThroughputMessage
        {
            get;
            set;
        }
    }
}