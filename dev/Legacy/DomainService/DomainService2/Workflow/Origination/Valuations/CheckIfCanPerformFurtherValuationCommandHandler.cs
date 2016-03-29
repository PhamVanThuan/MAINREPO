using DomainService2.SharedServices;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;

namespace DomainService2.Workflow.Origination.Valuations
{
    public class CheckIfCanPerformFurtherValuationCommandHandler : IHandlesDomainServiceCommand<CheckIfCanPerformFurtherValuationCommand>
    {
        private IX2WorkflowService x2WorkflowService;

        public CheckIfCanPerformFurtherValuationCommandHandler(IX2WorkflowService x2WorkflowService)
        {
            this.x2WorkflowService = x2WorkflowService;
        }

        public void Handle(IDomainMessageCollection messages, CheckIfCanPerformFurtherValuationCommand command)
        {
            bool HasDoneFurtherValuation = x2WorkflowService.HasInstancePerformedActivity(command.InstanceID, SAHL.Common.Constants.WorkFlowExternalActivity.PerformFurtherValuation);
            if (!HasDoneFurtherValuation)
                command.Result = true;
            else
            {
                command.Result = false;
                messages.Add(new Error("This case has already had a Further Valuation Performed", "This case has already had a Further Valuation Performed"));
            }
        }
    }
}