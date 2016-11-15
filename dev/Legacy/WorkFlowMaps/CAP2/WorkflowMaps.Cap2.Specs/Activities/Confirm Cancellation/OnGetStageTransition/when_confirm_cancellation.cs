using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Confirm_Cancellation.OnGetStageTransition
{
    [Subject("Activity => Confirm_Cancellation => OnGetStageTransition")] // AutoGenerated
    internal class when_confirm_cancellation : WorkflowSpecCap2
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Confirm_Cancellation(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}