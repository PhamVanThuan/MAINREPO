using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.LoanAdjustments.Specs.Activities.Decline_Term_Change.OnComplete
{
    [Subject("Activity => Decline_Term_Change => OnComplete")]
    internal class when_declining_term_change : WorkflowSpecLoanAdjustments
    {
        private static bool result;
        private static string expectedDeclineMessage;
        private static int expectedAccountKey;
        private static string expectedAdUsername;
        private static string activityDeclineMessage;
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
            //if send SendReminderEMail() is going fail for some reason then OnCompleteActivity_Decline_Term_Change will also fail.
            result
              = workflow.OnCompleteActivity_Decline_Term_Change(instanceData, workflowData, paramsData, messages, ref activityDeclineMessage);
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
            activityDeclineMessage.ShouldMatch(expectedDeclineMessage);
        };

        //Reminder email method is void can only test
        //it doesn't throw an exception, if it does then
        //activity return true will be false.
        private It should_send_reminder_email = () =>
        {
            commonClient.WasToldTo(x => x.SendReminderEMail((IDomainMessageCollection)messages, instanceData.ActivityADUserName, workflowData.AccountKey, expectedDeclineMessage));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}