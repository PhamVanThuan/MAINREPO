using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Client_Accepts.OnGetActivityMessage
{
    [Subject("Activity => Client_Accepts => OnGetActivityMessage")] // AutoGenerated
    internal class when_client_accepts : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Client_Accepts(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}