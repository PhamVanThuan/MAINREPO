using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2.Communication
{
    public interface IX2RequestCallback
    {
        void ReceiveRequest(IX2Request request);
    }
}