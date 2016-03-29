using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class AssignCreateorAsDynamicRoleCommandHandler : IHandlesDomainServiceCommand<AssignCreateorAsDynamicRoleCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public AssignCreateorAsDynamicRoleCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, AssignCreateorAsDynamicRoleCommand command)
        {
            string assignedTo = "";
            workflowAssignmentRepository.AssignCreateorAsDynamicRole(command.InstanceID, command.DynamicRole, out assignedTo, command.GenericKey, command.StateName);
            command.AssignedTo = assignedTo;
        }
    }
}