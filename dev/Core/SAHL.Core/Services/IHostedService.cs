using SAHL.Core.IoC;

namespace SAHL.Core.Services
{
    public enum HostedServiceRunningStatus
    {
        Stopped,
        Starting,
        Started,
        Stopping
    }

    public interface IHostedService : IStartableService, IStoppableService
    {
        string Name { get; }

        string Version { get; }

        HostedServiceRunningStatus RunningStatus { get; }
    }
}