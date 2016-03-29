using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.States.Application_Reactivated.OnAutoForward
{
    [Subject("State => Application_Reactiviated => OnAutoForward")]
    internal class when_application_reactivated : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
            workflowData.Last_State = "Application Capture";
        };

        private Because of = () =>
        {
            result = workflow.GetForwardStateName_Application_Reactivated(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_last_state_data_property = () =>
        {
            result.ShouldMatch(workflowData.Last_State);
        };
    }
}