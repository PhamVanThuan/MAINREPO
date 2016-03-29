using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.States.Manage_Lead.OnExit
{
    [Subject("State => Manage_Lead => OnExit")]
    internal class when_manage_lead : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            workflowData.Last_State = "test";
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Manage_Lead(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_last_state_property = () =>
        {
            workflowData.Last_State.ShouldMatch("Manage Lead");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}