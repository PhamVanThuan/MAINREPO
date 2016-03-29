using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReactiveUserOrRoundRobinForOSKeyCommandHandler : IHandlesDomainServiceCommand<ReactiveUserOrRoundRobinForOSKeyCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public ReactiveUserOrRoundRobinForOSKeyCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, ReactiveUserOrRoundRobinForOSKeyCommand command)
        {
            command.Result = workflowAssignmentRepository.ReactiveUserOrRoundRobin(command.DynamicRole, command.GenericKey, command.OrganisationStructureKey, command.InstanceID, command.StateName);
        }
    }
}