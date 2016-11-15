using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Send_to_Litigation.OnGetActivityMessage
{
    [Subject("Activity => Send_to_Litigation => OnGetActivityMessage")] // AutoGenerated
    internal class when_send_to_litigation : WorkflowSpecDebtCounselling
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Send_to_Litigation(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}