using System.Timers;

namespace SAHL.Services.PollingManager.Server
{
    public interface IPolledHandler
    {
        void Initialise();
        void OnTimedEvent(object source, ElapsedEventArgs e);
        void Start();
        void Stop();
        void HandlePolledEvent();
    }
}