using Machine.Specifications;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.LoanAdjustments.Specs.Activities.Approve_Term_Change.OnComplete
{
    [Subject("Activity => Approve_Term_Change => OnComplete")]
    internal class when_approve_term_change : WorkflowSpecLoanAdjustments
    {
        private static bool results;
        private static string message;
        private static string expectedActivityMessage;
        private static string expectedAdUsername;

        private Establish context = () =>
        {
            //what is expected
            expectedActivityMessage = "Review Decision";
            expectedAdUsername = "marchuanv";

            //set workflow or instance stuff
            message = string.Empty;
            ((InstanceDataStub)instanceData).ActivityADUserName = expectedAdUsername;
        };

        private Because of = () =>
        {
            results
                = workflow.OnCompleteActivity_Approve_Term_Change(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true_if_activity_completed = () =>
        {
            results.ShouldBeTrue();
        };

        private It should_set_request_user_data_to_aduser_name = () =>
        {
            workflowData.RequestUser.ShouldMatch(expectedAdUsername);
        };

        private It should_set_message_to_review_decision = () =>
        {
            message.ShouldMatch(expectedActivityMessage);
        };
    }
}