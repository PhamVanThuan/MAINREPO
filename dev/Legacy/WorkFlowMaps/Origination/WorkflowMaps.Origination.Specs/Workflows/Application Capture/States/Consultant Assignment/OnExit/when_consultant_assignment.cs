using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Consultant_Assignment.OnExit
{
    [Subject("State => Consultant_Assignment => OnExit")]
    internal class when_consultant_assignment : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.Last_State = "test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Consultant_Assignment(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_last_state_property = () =>
        {
            workflowData.Last_State.ShouldMatch("Consultant Assignment");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}