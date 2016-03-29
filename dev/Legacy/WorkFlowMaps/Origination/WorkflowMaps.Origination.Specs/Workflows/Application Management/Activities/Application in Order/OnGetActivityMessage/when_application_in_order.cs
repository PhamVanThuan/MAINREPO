using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Application_in_Order.OnGetActivityMessage
{
    [Subject("Activity => Application_in_Order => OnGetActivityMessage")] // AutoGenerated
    internal class when_application_in_order : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Application_in_Order(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}