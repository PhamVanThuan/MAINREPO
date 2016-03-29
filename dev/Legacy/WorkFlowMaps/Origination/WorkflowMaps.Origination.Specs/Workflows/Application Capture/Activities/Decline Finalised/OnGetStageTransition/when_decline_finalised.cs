using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Decline_Finalised.OnGetStageTransition
{
    [Subject("Activity => Decline_Finalised => OnGetStageTransition")]
    internal class when_decline_finalised : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Decline_Finalised(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_decline_finalised = () =>
        {
            result.ShouldEqual("Decline Finalised");
        };
    }
}