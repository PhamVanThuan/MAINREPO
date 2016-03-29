using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReassignToMostSenPersonWhoWorkedOnThisCaseInCreditCommandHandler : IHandlesDomainServiceCommand<ReassignToMostSenPersonWhoWorkedOnThisCaseInCreditCommand>
    {
        IWorkflowAssignmentRepository workflowAssignmentRepository;

        public ReassignToMostSenPersonWhoWorkedOnThisCaseInCreditCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, ReassignToMostSenPersonWhoWorkedOnThisCaseInCreditCommand command)
        {
            command.AssignedUserResult = workflowAssignmentRepository.ReassignToMostSenPersonWhoWorkedOnThisCaseInCredit(command.InstanceID, command.SourceInstanceID, command.ApplicationKey, command.State);
        }
    }
}