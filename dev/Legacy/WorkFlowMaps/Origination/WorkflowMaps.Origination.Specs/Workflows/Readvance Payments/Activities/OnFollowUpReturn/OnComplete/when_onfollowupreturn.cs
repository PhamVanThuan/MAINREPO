using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities.OnFollowUpReturn.OnComplete
{
    [Subject("Activity => OnFollowUpReturn => OnComplete")] // AutoGenerated
    internal class when_onfollowupreturn : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_OnFollowUpReturn(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}