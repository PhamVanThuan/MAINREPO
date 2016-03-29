using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Estate_Agent_Assignment.OnComplete
{
    [Subject("Activity => Estate_Agent_Assignment => OnComplete")] // AutoGenerated
    internal class when_estate_agent_assignment : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Estate_Agent_Assignment(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}