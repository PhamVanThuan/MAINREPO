using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2
{
    /// <summary>
    /// X2 Engine
    /// </summary>
    public interface IX2Engine
    {
        void Initialise();

        X2Response ReceiveRequest<T>(T request) where T : class, IX2Request;

        X2Response ReceiveExternalActivityRequest<T>(T request) where T : class, IX2ExternalActivityRequest;

        X2Response ReceiveSystemRequest<T>(T request) where T : class, IX2SystemRequest;

        X2Response ReceiveManagementMessage<T>(T message) where T : class, IX2NodeManagementMessage;
    }
}