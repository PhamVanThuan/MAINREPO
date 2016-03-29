using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class IsUserStillInSameOrgStructureForCaseReassignCommandHandler : IHandlesDomainServiceCommand<IsUserStillInSameOrgStructureForCaseReassignCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public IsUserStillInSameOrgStructureForCaseReassignCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, IsUserStillInSameOrgStructureForCaseReassignCommand command)
        {
            command.Result = workflowAssignmentRepository.IsUserStillInSameOrgStructureForCaseReassign(command.ADUserKey, command.DynamicRole, command.InstanceID);
        }
    }
}