using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Motivate.OnGetActivityMessage
{
    [Subject("Activity => Motivate => OnGetActivityMessage")] // AutoGenerated
    internal class when_motivate : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Motivate(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}