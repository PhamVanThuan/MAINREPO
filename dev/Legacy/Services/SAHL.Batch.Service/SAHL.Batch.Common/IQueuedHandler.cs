using SAHL.Shared.Messages;

namespace SAHL.Batch.Common
{
    public interface IQueuedHandler<TMessage> : IStoppableQueueHandler, IStartableQueueHandler
            where TMessage : class,IMessage
    {
        void HandleMessage(TMessage message);
    }
}