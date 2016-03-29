using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2
{
    public interface IX2WorkflowRequestHandler
    {
        X2Response Handle(IX2Request request);
    }
}