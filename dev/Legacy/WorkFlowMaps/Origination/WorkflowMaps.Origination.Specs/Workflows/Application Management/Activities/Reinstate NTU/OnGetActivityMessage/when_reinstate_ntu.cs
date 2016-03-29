using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Reinstate_NTU.OnGetActivityMessage
{
    [Subject("Activity => Reinstate_NTU => OnGetActivityMessage")] // AutoGenerated
    internal class when_reinstate_ntu : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Reinstate_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}