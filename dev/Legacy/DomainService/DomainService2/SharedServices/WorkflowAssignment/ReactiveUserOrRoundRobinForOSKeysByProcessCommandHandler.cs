using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReactiveUserOrRoundRobinForOSKeysByProcessCommandHandler : IHandlesDomainServiceCommand<ReactiveUserOrRoundRobinForOSKeysByProcessCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public ReactiveUserOrRoundRobinForOSKeysByProcessCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, ReactiveUserOrRoundRobinForOSKeysByProcessCommand command)
        {
            command.Result = workflowAssignmentRepository.ReactiveUserOrRoundRobin(command.DynamicRole, command.GenericKey, command.OrganisationStructureKeys, command.InstanceID, command.State, command.Process, command.RoundRobinPointerKey);
        }
    }
}