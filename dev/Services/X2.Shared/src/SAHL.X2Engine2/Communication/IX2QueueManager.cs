using SAHL.Core.X2.Messages;
using System.Collections.Generic;

namespace SAHL.X2Engine2.Communication
{
    public interface IX2QueueManager
    {
        Dictionary<X2Workflow, List<IX2RouteEndpoint>> DeclaredWorkflowRoutes { get; }

        void Initialise();
    }
}