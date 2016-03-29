using Machine.Specifications;

namespace SAHL.WorkflowMaps.ThirdPartyInvoices.Specs.States.Awaiting_Approval_for_Payment.OnExit
{
    [Subject("State => Awaiting_Approval_for_Payment => OnExit")] // AutoGenerated
    internal class when_awaiting_approval_for_payment : WorkflowSpecThirdPartyInvoices
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Awaiting_Approval_for_Payment(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}