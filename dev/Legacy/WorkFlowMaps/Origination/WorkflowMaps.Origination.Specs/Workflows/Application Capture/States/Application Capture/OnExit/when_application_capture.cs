using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.States.Application_Capture.OnExit
{
    [Subject("State => Application_Capture => OnExit")]
    internal class when_application_capture : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.Last_State = "abc";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Application_Capture(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_the_last_state_data_property = () =>
        {
            workflowData.Last_State.ShouldMatch("Application Capture");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}