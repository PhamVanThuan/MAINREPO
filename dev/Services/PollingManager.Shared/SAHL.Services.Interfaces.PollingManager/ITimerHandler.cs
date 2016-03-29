namespace SAHL.Services.Interfaces.PollingManager
{
    public interface ITimerHandler
    {
        void Initialise(int timerInterval);
        void Start();
        void Stop();
    }
}