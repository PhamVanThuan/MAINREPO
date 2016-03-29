using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.App_Not_Accepted_Finalised.OnComplete
{
    [Subject("Activity => App_Not_Accepted_Finalised => OnComplete")] // AutoGenerated
    internal class when_app_not_accepted_finalised : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_App_Not_Accepted_Finalised(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}