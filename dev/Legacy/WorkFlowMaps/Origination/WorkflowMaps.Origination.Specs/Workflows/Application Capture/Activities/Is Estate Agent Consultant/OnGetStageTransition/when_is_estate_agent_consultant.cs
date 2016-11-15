using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Is_Estate_Agent_Consultant.OnGetStageTransition
{
    [Subject("Activity => Is_Estate_Agent_Consultant => OnGetStageTransition")] // AutoGenerated
    internal class when_is_estate_agent_consultant : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Is_Estate_Agent_Consultant(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}