using SAHL.Core.X2.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Communication
{
    public interface IX2QueueNameBuilder
    {
        IX2RouteEndpoint GetUserQueue(Core.X2.Messages.X2Workflow workflow);

        IX2RouteEndpoint GetSystemQueue(Core.X2.Messages.X2Workflow workflow);

        IX2RouteEndpoint GetErrorQueue(Core.X2.Messages.X2Workflow workflow);

        List<IX2RouteEndpoint> GetQueues(X2Workflow workflow);

        List<string> GetExchanges(X2Workflow workflow);

        string GetUserExchange(Core.X2.Messages.X2Workflow workflow);

        string GetSystemExchange(Core.X2.Messages.X2Workflow workflow);
    }
}
