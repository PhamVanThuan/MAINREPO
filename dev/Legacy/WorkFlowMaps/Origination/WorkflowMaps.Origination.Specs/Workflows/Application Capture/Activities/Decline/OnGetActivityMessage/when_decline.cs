using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Decline.OnGetActivityMessage
{
    [Subject("Activity => Decline => OnGetActivityMessage")] // AutoGenerated
    internal class when_decline : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}