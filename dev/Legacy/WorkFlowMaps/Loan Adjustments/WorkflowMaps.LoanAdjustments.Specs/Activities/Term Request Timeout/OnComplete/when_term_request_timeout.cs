using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.LoanAdjustments.Specs.Activities.No_Longer_Required.OnComplete
{
    [Subject("Activity => Term_Request_Timeout => OnComplete")]
    internal class when_term_request_timeout : WorkflowSpecLoanAdjustments
    {
        private static bool result;
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
              = workflow.OnCompleteActivity_Term_Request_Timeout(instanceData, workflowData, paramsData, messages, ref message);
        };

        //Reminder email method is void can only test
        //it doesn't throw an exception, if it does then
        //activity return true will be false.
        private It should_send_reminder_email = () =>
        {
            //Only testing if SendReminderEMail() is there and can be invoked.
            commonClient.WasToldTo(x => x.SendReminderEMail((IDomainMessageCollection)messages, instanceData.ActivityADUserName, workflowData.AccountKey, "Not Yet Approved"));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}