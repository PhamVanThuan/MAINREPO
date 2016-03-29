using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReactivateUserOrLoadBalanceAssignCommandHandler : IHandlesDomainServiceCommand<ReactivateUserOrLoadBalanceAssignCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public ReactivateUserOrLoadBalanceAssignCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, ReactivateUserOrLoadBalanceAssignCommand command)
        {
            command.Result = workflowAssignmentRepository.ReactivateUserOrLoadBalanceAssign(command.UserOrganisationStructureGenericType, command.WorkflowRoleType, command.GenericKey, command.InstanceID, command.StatesToDetermineLoad, command.ProcessName, command.WorkflowName);
        }
    }
}