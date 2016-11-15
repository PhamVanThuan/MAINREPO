using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities.Request_Lightstone_Valuation.OnComplete
{
    [Subject("Activity => Request_Lightstone_Valuation => OnComplete")] // AutoGenerated
    internal class when_request_lightstone_valuation : WorkflowSpecReadvancePayments
    {
        static bool result;
        Establish context = () =>
        {
            result = true;
        };
		
        Because of = () =>
        {
			string message = string.Empty;
            result = workflow.OnCompleteActivity_Request_Lightstone_Valuation(instanceData, workflowData, paramsData, messages, message);
        };

        It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}