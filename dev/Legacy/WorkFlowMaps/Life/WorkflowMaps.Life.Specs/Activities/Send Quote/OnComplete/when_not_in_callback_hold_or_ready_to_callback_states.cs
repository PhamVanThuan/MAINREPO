using Machine.Specifications;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Life.Specs.Activities.Send_Quote.OnComplete
{
    [Subject("Activity => Send_Quote => OnComplete")]
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
            result = workflow.OnCompleteActivity_Send_Quote(instanceData, workflowData, paramsData, messages, ref message);
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