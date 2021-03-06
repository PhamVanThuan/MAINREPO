using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Rework_Application.OnGetActivityMessage
{
    [Subject("Activity => Rework_Application => OnGetActivityMessage")] // AutoGenerated
    internal class when_rework_application : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Rework_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}