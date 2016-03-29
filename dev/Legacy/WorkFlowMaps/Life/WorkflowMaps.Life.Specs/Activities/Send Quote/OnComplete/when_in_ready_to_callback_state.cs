using Machine.Specifications;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Life.Specs.Activities.Send_Quote.OnComplete
{
    [Subject("Activity => Send_Quote => OnComplete")]
    internal class when_in_ready_to_callback_state : WorkflowSpecLife
    {
        private static bool result;
        private static string message = string.Empty;

        private Establish context = () =>
            {
                ((ParamsDataStub)paramsData).StateName = "Ready to Callback";
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

        private It should_not_update_the_last_state_data_property = () =>
            {
                workflowData.LastState.ShouldNotBeTheSameAs(((ParamsDataStub)paramsData).StateName);
            };
    }
}