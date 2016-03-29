using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReactivateUserOrLoadBalanceAssignWithStatesCommandHandler : IHandlesDomainServiceCommand<ReactivateUserOrLoadBalanceAssignWithStatesCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public ReactivateUserOrLoadBalanceAssignWithStatesCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, ReactivateUserOrLoadBalanceAssignWithStatesCommand command)
        {
            command.Result = workflowAssignmentRepository.ReactivateUserOrLoadBalanceAssign(command.UserOrganisationStructureGenericType, command.WorkflowRoleType, command.GenericKey, command.InstanceID, command.StatesToDetermineLoad, command.ProcessName, command.WorkflowName, command.IncludeStates);
        }
    }
}