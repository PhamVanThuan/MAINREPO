using System;
using System.Threading;

namespace SAHL.Services.Cuttlefish.Managers
{
    public interface IQueueConsumer
    {
        void Consume();

        string QueueServerName { get; }

        string ExchangeName { get; }

        string QueueName { get; }

        string Username { get; }

        string Password { get; }

        Func<bool> ShouldCancel { get; }

        Action<string> WorkAction { get; }
    }
}