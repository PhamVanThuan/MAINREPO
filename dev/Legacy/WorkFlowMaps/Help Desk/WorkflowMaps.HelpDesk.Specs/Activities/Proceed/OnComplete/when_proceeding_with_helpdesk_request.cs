using Machine.Specifications;

namespace WorkflowMaps.HelpDesk.Specs.Activities.Proceed.OnComplete
{
    [Subject("Activity => Proceed => OnComplete")]
    internal class when_proceeding_with_helpdesk_request : WorkflowSpecHelpDesk
    {
        private static string expectedMessage;
        private static string message;
        private static bool result;

        private Establish context = () =>
            {
                expectedMessage = "Valid Call - Perform Actions";
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Proceed(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_set_activity_message = () =>
            {
                message.ShouldBeTheSameAs(expectedMessage);
            };
    }
}