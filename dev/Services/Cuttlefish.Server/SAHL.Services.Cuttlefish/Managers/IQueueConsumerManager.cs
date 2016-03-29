namespace SAHL.Services.Cuttlefish.Managers
{
    public interface IQueueConsumerManager
    {
        void StartConsumer(string queueExchangeName, string queueName, int numberOfWorkersToStart, IQueueConsumerWorker consumerWorker);

        int GetNumberOfRunningConsumers();

        void StopAllConsumers();
    }
}