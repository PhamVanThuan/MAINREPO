using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Dispute_Finalised.OnGetActivityMessage
{
    [Subject("Activity => Dispute_Finalised => OnGetActivityMessage")] // AutoGenerated
    internal class when_dispute_finalised : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Dispute_Finalised(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}