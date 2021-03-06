using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Resubmit_to_Credit.OnGetActivityMessage
{
    [Subject("Activity => Resubmit_to_Credit => OnGetActivityMessage")] // AutoGenerated
    internal class when_resubmit_to_credit : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Resubmit_to_Credit(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}