namespace SAHL.Core.Services.CommandPersistence
{
    public interface ICommandRetryPolicy
    {
        bool ShouldRetry();
    }
}