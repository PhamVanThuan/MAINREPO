using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Send_Decline_Letter.OnComplete
{
    [Subject("Activity => Send_Decline_Letter => OnComplete")] // AutoGenerated
    internal class when_send_decline_letter : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Send_Decline_Letter(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}