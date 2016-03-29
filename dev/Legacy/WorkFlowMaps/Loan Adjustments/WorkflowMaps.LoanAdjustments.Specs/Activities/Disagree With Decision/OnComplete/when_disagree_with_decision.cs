using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.LoanAdjustments.Specs.Activities.Disagree_With_Decision.OnComplete
{
    [Subject("Activity => Disagree_With_Decision => OnComplete")]
    internal class when_disagree_with_decision : WorkflowSpecLoanAdjustments
    {
        private static bool result;
        private static string expectedDeclineMessage;
        private static int expectedAccountKey;
        private static string expectedAdUsername;
        private static string message;
        private static ICommon commonClient;
        private static bool isRequestApproved;

        private Establish context = () =>
        {
            //Whats expected
            expectedAdUsername = "marchuanv";
            expectedAccountKey = 1234567;
            expectedDeclineMessage = "Request Declined by marchuanv";
            isRequestApproved = true;
            result = false;

            //Set workflow and instance properties
            commonClient = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
            ((InstanceDataStub)instanceData).ActivityADUserName = expectedAdUsername;
            workflowData.RequestApproved = isRequestApproved;
            workflowData.AccountKey = expectedAccountKey;
        };

        private Because of = () =>
        {
            result
              = workflow.OnCompleteActivity_Disagree_With_Decision(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_request_approved_to_false = () =>
        {
            workflowData.RequestApproved.ShouldBeFalse();
        };

        private It should_set_process_user_data_to_activity_aduser_name = () =>
        {
            //Code behind in map sets  workflowData.ProcessUser to instanceData.ActivityADUser
            workflowData.ProcessUser.ShouldMatch(expectedAdUsername);
        };

        private It should_set_message_to_request_decline_by_aduser_name = () =>
        {
            message.ShouldMatch(expectedDeclineMessage);
        };

        //Reminder email method is void can only test
        //it doesn't throw an exception, if it does then
        //activity return true will be false.
        private It should_send_reminder_email = () =>
        {
            //Only testing if SendReminderEMail() is there and can be invoked.
            commonClient.WasToldTo(x => x.SendReminderEMail((IDomainMessageCollection)messages, instanceData.CreatorADUserName, workflowData.AccountKey, expectedDeclineMessage));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}