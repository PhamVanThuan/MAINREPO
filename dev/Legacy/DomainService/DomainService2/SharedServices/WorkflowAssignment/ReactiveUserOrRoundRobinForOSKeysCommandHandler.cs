using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReactiveUserOrRoundRobinForOSKeysCommandHandler : IHandlesDomainServiceCommand<ReactiveUserOrRoundRobinForOSKeysCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public ReactiveUserOrRoundRobinForOSKeysCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, ReactiveUserOrRoundRobinForOSKeysCommand command)
        {
            command.Result = workflowAssignmentRepository.ReactiveUserOrRoundRobin(command.DynamicRole, command.GenericKey, command.OrganisationStructureKeys, command.InstanceID, command.StateName);
        }
    }
}