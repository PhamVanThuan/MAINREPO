using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReactivateUserOrRoundRobinForWorkflowRoleAssignmentCommandHandler : IHandlesDomainServiceCommand<ReactivateUserOrRoundRobinForWorkflowRoleAssignmentCommand>
    {
        IWorkflowRoleAssignmentRepository workflowRoleAssignmentRepository;

        public ReactivateUserOrRoundRobinForWorkflowRoleAssignmentCommandHandler(IWorkflowRoleAssignmentRepository workflowRoleAssignmentRepository)
        {
            this.workflowRoleAssignmentRepository = workflowRoleAssignmentRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, ReactivateUserOrRoundRobinForWorkflowRoleAssignmentCommand command)
        {
            command.AssignedUserResult = workflowRoleAssignmentRepository.ReactivateUserOrRoundRobin(command.GenericKeyType, command.WorkflowRoleType, command.GenericKey, command.InstanceID, command.RoundRobinPointer);
        }
    }
}