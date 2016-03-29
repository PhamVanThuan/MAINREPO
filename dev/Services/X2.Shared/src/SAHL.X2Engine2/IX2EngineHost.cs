using SAHL.Core.IoC;
using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2
{
    /// <summary>
    /// Abstraction of an X2 Engine that can be hosted in a variety of environents.
    /// e.g. A Service, In Memory, IIS or remoting
    /// </summary>
    public interface IX2EngineHost : IStartableService, IStoppableService
    {
        X2Response SendCreateWorkFlowInstanceRequest(X2CreateInstanceRequest request);

        X2Response SendCreateWorkFlowInstanceWithCompleteRequest(X2CreateInstanceWithCompleteRequest request);

        X2Response SendActivityStartRequest(X2RequestForExistingInstance request);

        X2Response SendActivityCompleteRequest(X2RequestForExistingInstance request);

        X2Response SendActivityCancelRequest(X2RequestForExistingInstance request);
    }
}