using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2
{
    /// <summary>
    /// Helper class that gets the process, workflow, state and activity for a request.
    /// </summary>
    public interface IX2RequestInterrogator
    {
        X2Workflow GetRequestWorkflow(IX2Request request);

        bool IsRequestMonitored(IX2Request request);
    }
}