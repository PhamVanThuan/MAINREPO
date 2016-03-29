using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class IsPolicyOverrideCommandHandler : IHandlesDomainServiceCommand<IsPolicyOverrideCommand>
    {
        IWorkflowAssignmentRepository workflowAssignmentRepo;

        public IsPolicyOverrideCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepo)
        {
            this.workflowAssignmentRepo = workflowAssignmentRepo;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, IsPolicyOverrideCommand command)
        {
            command.Result = workflowAssignmentRepo.IsPolicyOverride(command.InstanceID, command.SourceInstanceID, command.GenericKey);
        }
    }
}