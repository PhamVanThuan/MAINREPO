using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Send_Offer.OnGetActivityMessage
{
    [Subject("Activity => Send_Offer => OnGetActivityMessage")]
    internal class when_send_offer : WorkflowSpecPersonalLoans
    {
        private static string message;

        private Establish context = () =>
        {
            message = "test";
        };

        private Because of = () =>
        {
            message = workflow.GetActivityMessage_Send_Offer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            message.ShouldBeEmpty();
        };
    }
}