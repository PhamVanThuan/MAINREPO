using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class DeActiveUsersForInstanceAndProcessCommandHandler : IHandlesDomainServiceCommand<DeActiveUsersForInstanceAndProcessCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public DeActiveUsersForInstanceAndProcessCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, DeActiveUsersForInstanceAndProcessCommand command)
        {
            workflowAssignmentRepository.DeActiveUsersForInstance(command.InstanceID, command.GenericKey, command.DynamicRoles, command.Process);
        }
    }
}