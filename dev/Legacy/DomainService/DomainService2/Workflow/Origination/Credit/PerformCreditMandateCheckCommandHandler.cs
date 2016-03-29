using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.Origination.Credit
{
    public class PerformCreditMandateCheckCommandHandler : IHandlesDomainServiceCommand<PerformCreditMandateCheckCommand>
    {
        private IWorkflowAssignmentRepository workflowAssignmentRepository;

        //PerformCreditMandateCheck
        public PerformCreditMandateCheckCommandHandler(IWorkflowAssignmentRepository workflowAssignmentRepository)
        {
            this.workflowAssignmentRepository = workflowAssignmentRepository;
        }

        public void Handle(IDomainMessageCollection messages, PerformCreditMandateCheckCommand command)
        {
            workflowAssignmentRepository.PerformCreditMandateCheck(command.ApplicationKey,
                    command.InstanceID,
                    command.LoadBalanceStates,
                    command.LoadBalanceIncludeStates,
                    command.LoadBalance1stPass,
                    command.LoadBalance2ndPass);
        }
    }
}