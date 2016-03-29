using SAHL.Core.Services;
namespace SAHL.Core.X2
{
    public interface IX2Params
    {
        string ActivityName { get; }

        string StateName { get; }

        string WorkflowName { get; }

        string ADUserName { get; }

        bool IgnoreWarning { get; }

        object Data { get; }

        IServiceRequestMetadata ServiceRequestMetadata { get; }
    }
}