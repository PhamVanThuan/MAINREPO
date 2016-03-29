using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Is_Estate_Agent_Lead.OnComplete
{
    [Subject("Activity => Is_Estate_Agent_Lead => OnComplete")] // AutoGenerated
    internal class when_is_estate_agent_lead : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Is_Estate_Agent_Lead(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}