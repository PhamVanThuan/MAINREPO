namespace DomainService2.SharedServices.WorkflowAssignment
{
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;

    public class CheckUserInWorkflowRoleCommandHandler : IHandlesDomainServiceCommand<CheckUserInWorkflowRoleCommand>
    {
        private IWorkflowAssignmentRepository WorkflowAssignmentRepository;

        public CheckUserInWorkflowRoleCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.WorkflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, CheckUserInWorkflowRoleCommand command)
        {
            command.Result = WorkflowAssignmentRepository.CheckUserInWorkflowRole(command.ADUserName, command.WorkflowRoleTypeKey);
        }
    }
}