namespace SAHL.Batch.Common
{
    public interface IBatchServiceManager
    {
        void StartQueueHandlers();

        void StopQueueHandlers();
    }
}