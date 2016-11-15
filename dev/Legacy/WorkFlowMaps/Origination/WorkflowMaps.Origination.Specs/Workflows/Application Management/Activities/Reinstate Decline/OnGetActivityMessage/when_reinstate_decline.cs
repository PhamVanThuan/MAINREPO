using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Reinstate_Decline.OnGetActivityMessage
{
    [Subject("Activity => Reinstate_Decline => OnGetActivityMessage")] // AutoGenerated
    internal class when_reinstate_decline : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Reinstate_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}