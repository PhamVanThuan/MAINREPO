using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.CapitecOpenApplication.OnComplete
{
    [Subject("Activity => CapitecOpenApplication => OnComplete")] // AutoGenerated
    internal class when_capitecopenapplication : WorkflowSpecApplicationCapture
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
			string message = string.Empty;
            result = workflow.OnCompleteActivity_CapitecOpenApplication(instanceData, workflowData, paramsData, messages, ref message);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}