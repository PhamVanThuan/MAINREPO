using System;

namespace SAHL.Services.Cuttlefish.Managers
{
    public interface IQueueConnection : IDisposable
    {
        string QueueServerName { get; }

        string ExchangeName { get; }

        string QueueName { get; }

        string Username { get; }

        string Password { get; }

        object Dequeue();

        void SendAck(ulong deliveryTag);

        void SendNack(ulong deliveryTag, bool requeue);

        bool IsConnected { get; }
    }
}