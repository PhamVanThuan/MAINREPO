using System.Collections.Concurrent;
using System.Collections.Generic;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;

namespace SAHL.X2Engine2
{
    /// <summary>
    /// Component that uses a planner to determine the best route for a request
    /// and then send the request to the appropriate node.
    /// </summary>
    public interface IX2RequestRouter
    {
        void Initialise();

        void RouteRequest<T>(T request, IX2RequestMonitor requestMonitor) where T : class, IX2Request;
    }
}