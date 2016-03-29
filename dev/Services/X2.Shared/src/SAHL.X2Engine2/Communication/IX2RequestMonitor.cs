using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2.Communication
{
    /// <summary>
    /// Waits on x2 requests for a response.
    /// </summary>
    public interface IX2RequestMonitor
    {
        void MonitorRequest(IX2Request request);

        void Initialise(IX2RequestMonitorCallback requestCallback);

        void Reset(IX2Request request);

        void Stop();

        void HandleResponse(X2Response response);
    }
}