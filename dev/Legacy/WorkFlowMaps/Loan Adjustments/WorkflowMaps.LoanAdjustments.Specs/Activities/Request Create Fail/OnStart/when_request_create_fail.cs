using Machine.Specifications;

namespace WorkflowMaps.LoanAdjustments.Specs.Activities.Request_Create_Fail.OnStart
{
    [Subject("Activity => Request_Create_Fail => OnStart")] // AutoGenerated
    internal class when_request_create_fail : WorkflowSpecLoanAdjustments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Request_Create_Fail(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}