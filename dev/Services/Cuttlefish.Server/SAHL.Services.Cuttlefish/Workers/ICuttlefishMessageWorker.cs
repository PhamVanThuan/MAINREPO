namespace SAHL.Services.Cuttlefish.Worker
{
    public interface ICuttlefishMessageWorker
    {
        void ProcessMessage(string queueMessage);
    }
}