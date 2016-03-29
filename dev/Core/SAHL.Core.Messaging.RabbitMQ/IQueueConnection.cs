using RabbitMQ.Client;
using System;

namespace SAHL.Core.Messaging.RabbitMQ
{
    public interface IQueueConnection : IDisposable
    {
        void Connect();

        bool Reconnect();

        Guid ConnectionId { get; }

        IRabbitMQModel CreateModel();

        bool IsConnected();
    }
}