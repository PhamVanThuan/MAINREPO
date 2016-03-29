using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class GetLastUserToWorkOnCaseAcrossInstancesCommandHandler : IHandlesDomainServiceCommand<GetLastUserToWorkOnCaseAcrossInstancesCommand>
    {
        IWorkflowAssignmentRepository workflowAssignmentRepository;

        public GetLastUserToWorkOnCaseAcrossInstancesCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, GetLastUserToWorkOnCaseAcrossInstancesCommand command)
        {
            command.ADUserNameResult = workflowAssignmentRepository.GetLastUserToWorkOnCaseAcrossInstances(command.InstanceID, command.SourceInstanceID, command.OfferRoleTypeKey, command.DynamicRole, command.MapName);
        }
    }
}