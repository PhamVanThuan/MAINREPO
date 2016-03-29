namespace SAHL.Core.Services.CommandPersistence.CommandRetryPolicy
{
    public class CommandRetryPolicyThree : ICommandRetryPolicy
    {
        private int RetryCount = 3;

        private int FailedCount { get; set; }

        public bool ShouldRetry()
        {
            FailedCount++;
            return RetryCount >= FailedCount;
        }
    }
}