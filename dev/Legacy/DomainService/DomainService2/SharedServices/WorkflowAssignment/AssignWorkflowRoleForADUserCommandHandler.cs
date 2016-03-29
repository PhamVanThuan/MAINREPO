using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class AssignWorkflowRoleForADUserCommandHandler : IHandlesDomainServiceCommand<AssignWorkflowRoleForADUserCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public AssignWorkflowRoleForADUserCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, AssignWorkflowRoleForADUserCommand command)
        {
            workflowAssignmentRepository.AssignWorkflowRoleForADUser(command.InstanceID, command.AdUserName, command.WorkflowRoleType, command.GenericKey, command.State);
        }
    }
}