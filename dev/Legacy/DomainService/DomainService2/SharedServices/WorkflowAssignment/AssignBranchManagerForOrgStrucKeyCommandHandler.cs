using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class AssignBranchManagerForOrgStrucKeyCommandHandler : IHandlesDomainServiceCommand<AssignBranchManagerForOrgStrucKeyCommand>
    {
        IWorkflowAssignmentRepository workflowAssignmentRepo;

        public AssignBranchManagerForOrgStrucKeyCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepo)
        {
            this.workflowAssignmentRepo = workflowAssignmentRepo;
        }

        public void Handle(IDomainMessageCollection messages, AssignBranchManagerForOrgStrucKeyCommand command)
        {
            string assignedManager = workflowAssignmentRepo.AssignBranchManagerForOrgStrucKey(command.InstanceID,
                                                                                              command.DynamicRole,
                                                                                              command.OrganisationStructureKey,
                                                                                              command.GenericKey,
                                                                                              command.State,
                                                                                              command.Process);
            command.AssignedManagerResult = assignedManager;
        }
    }
}