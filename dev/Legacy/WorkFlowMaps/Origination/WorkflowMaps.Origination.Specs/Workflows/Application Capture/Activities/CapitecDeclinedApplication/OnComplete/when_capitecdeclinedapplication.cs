using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.CapitecDeclinedApplication.OnComplete
{
    [Subject("Activity => CapitecDeclinedApplication => OnComplete")] // AutoGenerated
    internal class when_capitecdeclinedapplication : WorkflowSpecApplicationCapture
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
			string message = string.Empty;
            result = workflow.OnCompleteActivity_CapitecDeclinedApplication(instanceData, workflowData, paramsData, messages, ref message);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}