using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReassignParentMapToCurrentUserCommandHandler : IHandlesDomainServiceCommand<ReassignParentMapToCurrentUserCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public ReassignParentMapToCurrentUserCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, ReassignParentMapToCurrentUserCommand command)
        {
            workflowAssignmentRepository.ReassignParentMapToCurrentUser(command.InstanceID, command.SourceInstanceID, command.ApplicationKey, command.State, command.Process);
        }
    }
}