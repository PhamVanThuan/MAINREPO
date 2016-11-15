using Machine.Specifications;

namespace WorkflowMaps.LoanAdjustments.Specs.Activities.Request_Create_Fail.OnComplete
{
    [Subject("Activity => Request_Create_Fail => OnComplete")] // AutoGenerated
    internal class when_request_create_fail : WorkflowSpecLoanAdjustments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Request_Create_Fail(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}