using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.WorkflowAssignmentDomain
{
    public interface IWorkflowAssignmentDomainServiceClient
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IWorkflowAssignmentDomainCommand;

        ISystemMessageCollection PerformQuery<T>(T query) where T : IWorkflowAssignmentDomainQuery;

        event SAHL.Core.Web.Services.ServiceHttpClient.CurrentlyPerformingRequestHandler CurrentlyPerformingRequest;
    }
}
