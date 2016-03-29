using SAHL.X2Engine2.Communication;

namespace SAHL.X2Engine2
{
    public interface IX2RequestMonitorFactory
    {
        IX2RequestMonitor GetOrCreateRequestMonitor(IResponseThreadWaiter threadWaiter, IX2RequestMonitorCallback callback);

        IX2RequestMonitor GetRequestMonitor(IResponseThreadWaiter threadWaiter);

        void RemoveRequestMonitor(IResponseThreadWaiter threadWaiter);

        void Initialise();
    }
}