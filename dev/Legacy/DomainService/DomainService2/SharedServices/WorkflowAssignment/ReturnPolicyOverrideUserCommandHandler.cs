using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReturnPolicyOverrideUserCommandHandler : IHandlesDomainServiceCommand<ReturnPolicyOverrideUserCommand>
    {
        IWorkflowAssignmentRepository workflowAssignmentRepository;

        public ReturnPolicyOverrideUserCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, ReturnPolicyOverrideUserCommand command)
        {
            command.UserResult = workflowAssignmentRepository.ReturnPolicyOverrideUser(command.InstanceID);
        }
    }
}