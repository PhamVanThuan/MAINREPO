using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReassignCaseToUserCommandHandler : IHandlesDomainServiceCommand<ReassignCaseToUserCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public ReassignCaseToUserCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, ReassignCaseToUserCommand command)
        {
            workflowAssignmentRepository.ReassignCaseToUser(command.InstanceID, command.GenericKey, command.AdUserName, command.OrganisationStructureKey, command.OfferRoleTypeKey, command.StateName);
        }
    }
}