using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.Activities.Escalate_to_Manager.OnGetActivityMessage
{
    [Subject("Activity => Escalate_to_Manager => OnGetActivityMessage")] // AutoGenerated
    internal class when_escalate_to_manager : WorkflowSpecValuations
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Escalate_to_Manager(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}