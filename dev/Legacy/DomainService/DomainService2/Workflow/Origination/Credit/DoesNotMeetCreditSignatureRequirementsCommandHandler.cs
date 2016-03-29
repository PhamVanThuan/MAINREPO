using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.Credit
{
    public class CreditDecisionCheckAuthorisationRulesCommandHandler : IHandlesDomainServiceCommand<DoesNotMeetCreditSignatureRequirementsCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        public CreditDecisionCheckAuthorisationRulesCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, DoesNotMeetCreditSignatureRequirementsCommand command)
        {
            command.Result = workflowAssignmentRepository.CreditDecisionCheckAuthorisationRules(command.ApplicationKey, command.InstanceID);
        }
    }
}