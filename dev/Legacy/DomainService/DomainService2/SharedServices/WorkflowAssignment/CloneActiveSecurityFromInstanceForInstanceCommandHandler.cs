using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class CloneActiveSecurityFromInstanceForInstanceCommandHandler : IHandlesDomainServiceCommand<CloneActiveSecurityFromInstanceForInstanceCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public CloneActiveSecurityFromInstanceForInstanceCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, CloneActiveSecurityFromInstanceForInstanceCommand command)
        {
            command.Result = workflowAssignmentRepository.CloneActiveSecurityFromInstanceForInstance(command.ParentInstanceID, command.InstanceID);
        }
    }
}