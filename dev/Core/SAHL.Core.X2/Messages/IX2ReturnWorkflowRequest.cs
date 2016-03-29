namespace SAHL.Core.X2.Messages
{
    public interface IX2ReturnWorkflowRequest
    {
        long SourceInstanceId { get; }

        int ReturnActivityId { get; }
    }
}