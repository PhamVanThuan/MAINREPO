using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Capitec_Applications.OnExit
{
    [Subject("State => Capitec_Applications => OnExit")]
    internal class when_capitec_applications : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.Last_State = "test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Capitec_Applications(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_last_state_property = () =>
        {
            workflowData.Last_State.ShouldMatch("Capitec Applications");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}