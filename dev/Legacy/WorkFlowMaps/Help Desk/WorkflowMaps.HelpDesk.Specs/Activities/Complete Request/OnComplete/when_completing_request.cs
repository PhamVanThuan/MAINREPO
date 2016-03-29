using Machine.Specifications;
using System;

namespace WorkflowMaps.HelpDesk.Specs.Activities.Complete_Request.OnComplete
{
    [Subject("Activity => Complete_Request => OnComplete")]
    internal class when_completing_request : WorkflowSpecHelpDesk
    {
        private static string expectedMessage;
        private static String message;
        private static bool result;

        private Establish context = () =>
        {
            expectedMessage = "Call closed";
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Complete_Request(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_set_activity_message_to_call_closed = () =>
        {
            message.ShouldBeTheSameAs(expectedMessage);
        };
    }
}