using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Query_on_LOA.OnGetActivityMessage
{
    [Subject("Activity => Query_on_LOA => OnGetActivityMessage")] // AutoGenerated
    internal class when_query_on_loa : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Query_on_LOA(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}