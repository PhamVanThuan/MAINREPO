using System;

namespace SAHL.Services.Cuttlefish.Worker.Shared
{
    public interface IMessageWorkerManager
    {
        void StartWorker(string messageExchangeName, string messageQueueName, int numberOfWorkersToStart, Action<string> workerAction);
    }
}