using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Reactivate__App_Not_Accepted.OnGetActivityMessage
{
    [Subject("Activity => Reactivate__App_Not_Accepted => OnGetActivityMessage")] // AutoGenerated
    internal class when_reactivate__app_not_accepted : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Reactivate__App_Not_Accepted(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}