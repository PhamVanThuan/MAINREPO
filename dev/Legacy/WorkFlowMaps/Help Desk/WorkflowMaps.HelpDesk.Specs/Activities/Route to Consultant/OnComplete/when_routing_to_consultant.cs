using Machine.Specifications;

namespace WorkflowMaps.HelpDesk.Specs.Activities.Route_to_Consultant.OnComplete
{
    [Subject("Activity => Route_to_Consultant => OnComplete")]
    internal class when_routing_to_consultant : WorkflowSpecHelpDesk
    {
        private static string expectedMessage;
        private static string message;
        private static bool result;

        private Establish context = () =>
        {
            expectedMessage = "Call routed to you";
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Route_to_Consultant(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_set_activity_message_to_call_routed_to_you = () =>
        {
            message.ShouldBeTheSameAs(expectedMessage);
        };
    }
}