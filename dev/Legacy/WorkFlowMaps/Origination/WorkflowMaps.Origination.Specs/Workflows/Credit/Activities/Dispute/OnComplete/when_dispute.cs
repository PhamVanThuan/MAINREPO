using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.Activities.Dispute.OnComplete
{
    [Subject("Activity => Dispute => OnComplete")] // AutoGenerated
    internal class when_dispute : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Dispute(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}