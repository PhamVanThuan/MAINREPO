namespace SAHL.Core.X2.Messages
{
    public interface IX2ExternalActivityRequest : IX2SystemRequest
    {
        long? ActivatingInstanceId { get; }

        int ExternalActivityId { get; set; }

        int WorkflowId { get; set; }
    }
}