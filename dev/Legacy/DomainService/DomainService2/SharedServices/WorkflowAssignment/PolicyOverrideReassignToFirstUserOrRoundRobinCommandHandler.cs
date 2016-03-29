using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class PolicyOverrideReassignToFirstUserOrRoundRobinCommandHandler : IHandlesDomainServiceCommand<PolicyOverrideReassignToFirstUserOrRoundRobinCommand>
    {
        IWorkflowAssignmentRepository workflowAssignmentRepo;

        public PolicyOverrideReassignToFirstUserOrRoundRobinCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepo)
        {
            this.workflowAssignmentRepo = workflowAssignmentRepo;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, PolicyOverrideReassignToFirstUserOrRoundRobinCommand command)
        {
            command.AssignedUserResult = workflowAssignmentRepo.PolicyOverrideReassignToFirstUserOrRoundRobin(command.InstanceID, command.SourceInstanceID, command.GenericKey, command.State, command.Process);
        }
    }
}