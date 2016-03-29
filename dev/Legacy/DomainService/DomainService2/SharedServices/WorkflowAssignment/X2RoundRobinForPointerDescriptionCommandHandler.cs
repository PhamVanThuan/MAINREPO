using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class X2RoundRobinForPointerDescriptionCommandHandler : IHandlesDomainServiceCommand<X2RoundRobinForPointerDescriptionCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public X2RoundRobinForPointerDescriptionCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, X2RoundRobinForPointerDescriptionCommand command)
        {
            command.Result = workflowAssignmentRepository.X2RoundRobinForPointerDescription(command.InstanceID, command.RoundRobinPointerKey, command.GenericKey, command.DynamicRole, command.State, command.Process);
        }
    }
}