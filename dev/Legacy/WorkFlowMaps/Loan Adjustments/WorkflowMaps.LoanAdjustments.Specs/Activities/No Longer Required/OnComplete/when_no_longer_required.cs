using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.LoanAdjustments.Specs.Activities.No_Longer_Required.OnComplete
{
    [Subject("Activity => No_Longer_Required => OnComplete")]
    internal class when_no_longer_required : WorkflowSpecLoanAdjustments
    {
        private static bool result;
        private static string expectedMessage;
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
            expectedMessage = "No Longer Required marchuanv";
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
              = workflow.OnCompleteActivity_No_Longer_Required(instanceData, workflowData, paramsData, messages, ref message);
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

        //Reminder email method is void can only test
        //it doesn't throw an exception, if it does then
        //activity return true will be false.
        private It should_send_reminder_email = () =>
        {
            //Only testing if SendReminderEMail() is there and can be invoked.
            commonClient.WasToldTo(x => x.SendReminderEMail((IDomainMessageCollection)messages, instanceData.ActivityADUserName, workflowData.AccountKey, expectedMessage));
        };

        private It should_set_message_to_no_longer_required_by_aduser_name = () =>
        {
            message.ShouldMatch(expectedMessage);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}