using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Send_Offer.OnComplete
{
    [Subject("Activity => Send_Offer => OnComplete")]
    internal class when_send_offer : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Send_Offer(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}