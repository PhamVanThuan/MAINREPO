using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Assign_Admin.OnGetActivityMessage
{
    [Subject("Activity => Assign_Admin => OnGetActivityMessage")] // AutoGenerated
    internal class when_assign_admin : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Assign_Admin(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}