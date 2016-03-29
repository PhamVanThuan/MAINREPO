namespace SAHL.Services.Cuttlefish.Managers
{
    public interface IQueueConsumerWorker
    {
        void ProcessMessage(string queueMessage);
    }
}