using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ResolveDynamicRoleToUserNameCommandHandler : IHandlesDomainServiceCommand<ResolveDynamicRoleToUserNameCommand>
    {
        IWorkflowAssignmentRepository workflowAssignmentRepository;

        public ResolveDynamicRoleToUserNameCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, ResolveDynamicRoleToUserNameCommand command)
        {
            command.ADUserNameResult = workflowAssignmentRepository.ResolveDynamicRoleToUserName(command.DynamicRole, command.InstanceID);
        }
    }
}