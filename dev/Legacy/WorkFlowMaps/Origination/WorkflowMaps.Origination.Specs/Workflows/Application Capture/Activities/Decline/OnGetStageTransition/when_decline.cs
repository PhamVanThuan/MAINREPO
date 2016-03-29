using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Decline.OnGetStageTransition
{
    [Subject("Activity => Decline => OnGetStageTransition")]
    internal class when_decline : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_decline = () =>
        {
            result.ShouldEqual("Decline");
        };
    }
}