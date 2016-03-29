using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class AssignCaseThatWasPreviouslyInDisputeIndicatedCommandHandler : IHandlesDomainServiceCommand<AssignCaseThatWasPreviouslyInDisputeIndicatedCommand>
    {
        IWorkflowAssignmentRepository workflowAssignmentRepo;

        public AssignCaseThatWasPreviouslyInDisputeIndicatedCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepo)
        {
            this.workflowAssignmentRepo = workflowAssignmentRepo;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, AssignCaseThatWasPreviouslyInDisputeIndicatedCommand command)
        {
            command.AssignedToResult = workflowAssignmentRepo.AssignCaseThatWasPreviouslyInDisputeIndicated(command.ApplicationKey, command.InstanceID);
        }
    }
}