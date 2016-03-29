using System.Collections.Concurrent;
using System.Collections.Generic;
using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2
{
    /// <summary>
    /// Component that holds the list of available routes.
    /// It also collects the health management messages to update the statistics.
    /// It also determines the best route for a request for the engine using a route selector.
    /// </summary>
    public interface IX2RoutePlanner
    {
        void Initialise();

        IX2RouteEndpoint PlanRoute(bool monitoredRequest, X2Workflow workflow);
    }
}