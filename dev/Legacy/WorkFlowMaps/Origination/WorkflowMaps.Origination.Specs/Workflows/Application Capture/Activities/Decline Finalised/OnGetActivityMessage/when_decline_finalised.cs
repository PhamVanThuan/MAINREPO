using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Decline_Finalised.OnGetActivityMessage
{
    [Subject("Activity => Decline_Finalised => OnGetActivityMessage")] // AutoGenerated
    internal class when_decline_finalised : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Decline_Finalised(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}