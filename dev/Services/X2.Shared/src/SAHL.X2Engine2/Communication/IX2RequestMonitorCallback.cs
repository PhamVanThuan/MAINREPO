using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2.Communication
{
    /// <summary>
    /// The callbacks called when a request completes.
    /// </summary>
    public interface IX2RequestMonitorCallback
    {
        void RequestCompleted(X2Response response);

        void RequestTimedout(IX2Request request, IResponseThreadWaiter threadWaiter);
    }
}