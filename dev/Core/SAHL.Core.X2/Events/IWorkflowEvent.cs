using SAHL.Core.Events;

namespace SAHL.Core.X2.Events
{
    public interface IWorkflowEvent : IEvent
    {
        long InstanceId { get; }

        string ProcessName { get; }

        string WorkflowName { get; }

        string AdUserName { get; }
    }
}