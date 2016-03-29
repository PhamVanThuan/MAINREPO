using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Create_Clone.OnStart
{
    [Subject("Activity => Create_Clone => OnStart")]
    internal class when_create_clone_last_state_invalid_app_hold : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            workflowData.Last_State = "InvalidAppHold";
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Create_Clone(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}