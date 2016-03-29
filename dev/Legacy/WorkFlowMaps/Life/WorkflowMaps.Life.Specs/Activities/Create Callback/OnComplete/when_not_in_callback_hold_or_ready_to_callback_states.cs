using Machine.Specifications;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Life.Specs.Activities.Create_Callback.OnComplete
{
    [Subject("Activity => Create_Callback => OnComplete")]
    internal class when_not_in_callback_hold_or_ready_to_callback_states : WorkflowSpecLife
    {
        private static bool result;
        private static string message = string.Empty;

        private Establish context = () =>
        {
            ((ParamsDataStub)paramsData).StateName = "StateName";
            result = false;
            workflowData.LastState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Create_Callback(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_update_the_last_state_data_property = () =>
        {
            workflowData.LastState.ShouldBeTheSameAs(((ParamsDataStub)paramsData).StateName);
        };
    }
}