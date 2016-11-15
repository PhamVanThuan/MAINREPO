using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Is_Comcorp.OnComplete
{
    [Subject("Activity => Is_Comcorp => OnComplete")] // AutoGenerated
    internal class when_is_comcorp : WorkflowSpecApplicationCapture
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
			string message = string.Empty;
            result = workflow.OnCompleteActivity_Is_Comcorp(instanceData, workflowData, paramsData, messages, ref message);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}