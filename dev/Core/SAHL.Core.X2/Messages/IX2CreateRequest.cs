namespace SAHL.Core.X2.Messages
{
    public interface IX2CreateRequest : IX2UserRequest
    {
        string WorkflowName { get; }

        string ProcessName { get; }

        int? ReturnActivityId { get; }

        long? SourceInstanceId { get; }
    }
}