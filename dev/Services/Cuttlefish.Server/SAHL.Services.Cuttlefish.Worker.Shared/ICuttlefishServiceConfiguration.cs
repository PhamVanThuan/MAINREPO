namespace SAHL.Services.Cuttlefish.Worker.Shared
{
    public interface ICuttlefishServiceConfiguration
    {
        int WorkerCountForv2LogMessage { get; }

        int WorkerCountForv3LogMessage { get; }

        int WorkerCountForv2ThroughputMessage { get; }

        int WorkerCountForv3ThroughputMessage { get; }

        int WorkerCountForv2LatencyMessage { get; }

        int WorkerCountForv3LatencyMessage { get; }

        string MessageBusServer { get; }

        string MessageBusServerUserName { get; }

        string MessageBusServerPassword { get; }
    }
}