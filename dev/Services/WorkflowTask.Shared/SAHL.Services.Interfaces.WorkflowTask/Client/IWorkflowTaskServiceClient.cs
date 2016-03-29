using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.WorkflowTask
{
    public interface IWorkflowTaskServiceClient
    {
        ISystemMessageCollection PerformQuery<T>(T query) where T : IWorkflowTaskQuery;
    }
}