using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReactiveUserOrRoundRobinForOSKeyByProcessCommandHandler : IHandlesDomainServiceCommand<ReactiveUserOrRoundRobinForOSKeyByProcessCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public ReactiveUserOrRoundRobinForOSKeyByProcessCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, ReactiveUserOrRoundRobinForOSKeyByProcessCommand command)
        {
            command.Result = workflowAssignmentRepository.ReactiveUserOrRoundRobin(command.DynamicRole, command.GenericKey, command.OrganisationStructureKey, command.InstanceID, command.State, command.Process, command.RoundRobinPointerKey);
        }
    }
}