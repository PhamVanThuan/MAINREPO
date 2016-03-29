using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class DeActiveUsersForInstanceCommandHandler : IHandlesDomainServiceCommand<DeActiveUsersForInstanceCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public DeActiveUsersForInstanceCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, DeActiveUsersForInstanceCommand command)
        {
            workflowAssignmentRepository.DeActiveUsersForInstance(command.InstanceID, command.GenericKey, command.DynamicRoles);
        }
    }
}