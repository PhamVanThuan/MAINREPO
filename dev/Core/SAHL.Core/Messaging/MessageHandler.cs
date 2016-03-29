using System;
using SAHL.Core.Messaging.Shared;

namespace SAHL.Core.Messaging
{
    public class MessageHandler
    {
        private readonly Action<IMessage> handler;

        public string Topic { get; private set; }

        public MessageHandler(Action<IMessage> handler, string topic)
        {
            this.handler = handler;
            Topic = topic;
        }

        public void HandleCommand(IMessage message)
        {
            this.handler.Invoke(message);
        }
    }
}