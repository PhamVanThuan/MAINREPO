using System;
using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2
{
    /// <summary>
    /// Stores information persistently about the state of requests made to X2
    /// </summary>
    public interface IX2RequestStore
    {
        void StoreReceivedRequest(IX2Request request);

        void RemoveCompletedRequest(Guid requestID);

        void UpdateReceivedRequestAsRouted(IX2Request request);

        void UpdateReceivedRequestAsTimedoutAndWaiting(IX2Request request);

        void UpdateReceivedRequestAsTimedoutAndNotServicable(IX2Request request);

        void StoreReceivedRequestAsUnserviceableDueToNoAvailableRoute(IX2Request request);

        int GetNumberOfTimeouts(IX2Request request);
    }
}