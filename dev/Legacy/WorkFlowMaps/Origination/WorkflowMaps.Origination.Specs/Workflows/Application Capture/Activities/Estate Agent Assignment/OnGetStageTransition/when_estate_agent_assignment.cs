using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Estate_Agent_Assignment.OnGetStageTransition
{
    [Subject("Activity => Estate_Agent_Assignment => OnGetStageTransition")]
    internal class when_estate_agent_assignment : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Estate_Agent_Assignment(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_estate_agent_assignment = () =>
        {
            result.ShouldEqual("Estate Agent Assignment");
        };
    }
}