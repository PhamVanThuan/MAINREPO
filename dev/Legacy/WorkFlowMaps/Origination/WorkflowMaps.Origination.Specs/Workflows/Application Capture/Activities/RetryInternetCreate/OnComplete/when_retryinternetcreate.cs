using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.RetryInternetCreate.OnComplete
{
    [Subject("Activity => RetryInternetCreate => OnComplete")] // AutoGenerated
    internal class when_retryinternetcreate : WorkflowSpecApplicationCapture
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
			string message = string.Empty;
            result = workflow.OnCompleteActivity_RetryInternetCreate(instanceData, workflowData, paramsData, messages, ref message);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}