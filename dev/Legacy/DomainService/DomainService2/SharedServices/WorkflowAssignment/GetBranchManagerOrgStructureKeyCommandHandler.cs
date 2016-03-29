using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class GetBranchManagerOrgStructureKeyCommandHandler : IHandlesDomainServiceCommand<GetBranchManagerOrgStructureKeyCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public GetBranchManagerOrgStructureKeyCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, GetBranchManagerOrgStructureKeyCommand command)
        {
            command.OrganisationStructureKeyResult = workflowAssignmentRepository.GetBranchManagerOrgStructureKey(command.InstanceID);
        }
    }
}