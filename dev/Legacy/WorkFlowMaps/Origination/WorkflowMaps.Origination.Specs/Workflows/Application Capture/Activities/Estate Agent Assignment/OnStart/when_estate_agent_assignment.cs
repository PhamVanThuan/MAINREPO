using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Estate_Agent_Assignment.OnStart
{
    [Subject("Activity => Estate_Agent_Assignment => OnStart")] // AutoGenerated
    internal class when_estate_agent_assignment : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Estate_Agent_Assignment(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}