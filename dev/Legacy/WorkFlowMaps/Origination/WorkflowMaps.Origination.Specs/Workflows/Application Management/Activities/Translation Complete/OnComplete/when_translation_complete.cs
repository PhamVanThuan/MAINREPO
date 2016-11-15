using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Translation_Complete.OnComplete
{
    [Subject("Activity => Translation_Complete => OnComplete")] // AutoGenerated
    internal class when_translation_complete : WorkflowSpecApplicationManagement
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
			string message = string.Empty;
            result = workflow.OnCompleteActivity_Translation_Complete(instanceData, workflowData, paramsData, messages, ref message);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}