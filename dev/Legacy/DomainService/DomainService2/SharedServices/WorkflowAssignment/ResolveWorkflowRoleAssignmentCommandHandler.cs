using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ResolveWorkflowRoleAssignmentCommandHandler : IHandlesDomainServiceCommand<ResolveWorkflowRoleAssignmentCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public ResolveWorkflowRoleAssignmentCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, ResolveWorkflowRoleAssignmentCommand command)
        {
            command.ADUserNameResult = workflowAssignmentRepository.ResolveWorkflowRoleAssignment(command.InstanceID, command.WorkflowRoleType, command.WorkflowRoleTypeGroup);
        }
    }
}