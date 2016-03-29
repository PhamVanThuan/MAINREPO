using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class AssignWorkflowRoleCommandHandler : IHandlesDomainServiceCommand<AssignWorkflowRoleCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public AssignWorkflowRoleCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, AssignWorkflowRoleCommand command)
        {
            workflowAssignmentRepository.AssignWorkflowRole(command.InstanceID, command.AdUserKey, command.OfferRoleTypeOrganisationStructureMappingKey, command.StateName);
        }
    }
}