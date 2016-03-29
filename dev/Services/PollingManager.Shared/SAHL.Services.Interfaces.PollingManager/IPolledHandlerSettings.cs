namespace SAHL.Services.Interfaces.PollingManager
{
    public interface IPolledHandlerSettings
    {
        int TimerInterval { get; }
        int ProcessingSetSize { get; }
    }
}