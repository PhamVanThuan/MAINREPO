namespace SAHL.Services.Cuttlefish.Worker.Shared.Workers
{
    public interface ICuttlefishMessageWorker
    {
        void ProcessMessage(string queueMessage);
    }
}