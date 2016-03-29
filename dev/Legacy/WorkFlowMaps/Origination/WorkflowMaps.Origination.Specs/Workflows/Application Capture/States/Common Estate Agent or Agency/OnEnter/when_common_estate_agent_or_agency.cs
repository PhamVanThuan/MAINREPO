using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Common_Estate_Agent_or_Agency.OnEnter
{
    [Subject("State => Common_Estate_Agent_or_Agency => OnEnter")] // AutoGenerated
    internal class when_common_estate_agent_or_agency : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Common_Estate_Agent_or_Agency(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}