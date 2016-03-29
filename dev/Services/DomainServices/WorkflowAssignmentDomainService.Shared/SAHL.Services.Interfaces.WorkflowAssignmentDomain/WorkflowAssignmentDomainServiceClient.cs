using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;


namespace SAHL.Services.Interfaces.WorkflowAssignmentDomain
{
    public class WorkflowAssignmentDomainServiceClient : ServiceHttpClientWindowsAuthenticated, IWorkflowAssignmentDomainServiceClient
    {
        public WorkflowAssignmentDomainServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IWorkflowAssignmentDomainCommand
        {
            return base.PerformCommandInternal<T>(command, metadata);
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : IWorkflowAssignmentDomainQuery
        {
            return base.PerformQueryInternal<T>(query);
        }
    }
}
