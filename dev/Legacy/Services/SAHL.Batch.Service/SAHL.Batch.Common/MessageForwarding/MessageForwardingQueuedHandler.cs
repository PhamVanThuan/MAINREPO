using SAHL.Core.Messaging.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Batch.Common.MessageForwarding
{
    public class MessageForwardingQueuedHandler<TMessage, TMessageToForward> : IForwardingQueuedHandler<TMessage, TMessageToForward> where TMessage:class where TMessageToForward : class, SAHL.Shared.Messages.IMessage
    {
        private readonly IForwardingMessageBus messageBus;
        private Func<TMessage, TMessageToForward> messageTranformer;

        public MessageForwardingQueuedHandler(IForwardingMessageBus messageBus)
        {
            this.messageBus = messageBus;
            this.messageBus.Subscribe<TMessage>(Subscribe);
        }

        public void Subscribe(TMessage message)
        {
            var messageToForward = this.messageTranformer(message);
            this.messageBus.Publish<TMessageToForward>(messageToForward);
        }

        public void Stop()
        {

        }

        public void Start()
        {

        }

        public void Transform(Func<TMessage, TMessageToForward> transformFunc)
        {
            this.messageTranformer = transformFunc;
        }
    }
}
