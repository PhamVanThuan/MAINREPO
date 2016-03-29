using System;

namespace SAHL.Core.Messaging.RabbitMQ
{
    public interface IQueueConsumer
    {
        string ConsumerId { get; }

        bool IsConnected { get; }

        Func<bool> ShouldCancel { get; }

        Action<string> WorkAction { get; }

        void Consume();
    }
}