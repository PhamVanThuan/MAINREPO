namespace SAHL.Core.Services.CommandPersistence.CommandRetryPolicy
{
    public class CommandRetryPolicyNone : ICommandRetryPolicy
    {
        public bool ShouldRetry()
        {
            return false;
        }
    }
}