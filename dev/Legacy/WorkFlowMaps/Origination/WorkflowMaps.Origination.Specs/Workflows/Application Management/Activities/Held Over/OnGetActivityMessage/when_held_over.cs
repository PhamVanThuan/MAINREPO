using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Held_Over.OnGetActivityMessage
{
    [Subject("Activity => Held_Over => OnGetActivityMessage")] // AutoGenerated
    internal class when_held_over : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Held_Over(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}