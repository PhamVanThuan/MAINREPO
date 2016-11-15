using Machine.Specifications;

namespace WorkflowMaps.LoanAdjustments.Specs.States.Request_Failed.OnEnter
{
    [Subject("State => Request_Failed => OnEnter")] // AutoGenerated
    internal class when_request_failed : WorkflowSpecLoanAdjustments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Request_Failed(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}