using SAHL.Shared.Messages;

namespace SAHL.Batch.Common
{
    public interface IMessageProcessor<TMessage> where TMessage : class, IMessage
    {
        bool Process(TMessage message);
    }
}