using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReassignCaseToUserByProcessCommandHandler : IHandlesDomainServiceCommand<ReassignCaseToUserByProcessCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public ReassignCaseToUserByProcessCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, ReassignCaseToUserByProcessCommand command)
        {
            workflowAssignmentRepository.ReassignCaseToUser(command.InstanceID, command.GenericKey, command.AdUserName, command.OrganisationStructureKey, command.OfferRoleTypeKey, command.StateName, command.ProcessName);
        }
    }
}