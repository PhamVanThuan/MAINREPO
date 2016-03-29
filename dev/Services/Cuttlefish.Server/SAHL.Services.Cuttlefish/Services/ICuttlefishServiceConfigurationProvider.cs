namespace SAHL.Services.Cuttlefish.Services
{
    public interface ICuttlefishServiceConfigurationProvider
    {
        int WorkerCountForv2LogMessage { get; }

        int WorkerCountForv2ThroughputMessage { get; }

        int WorkerCountForv2LatencyMessage { get; }
    }
}