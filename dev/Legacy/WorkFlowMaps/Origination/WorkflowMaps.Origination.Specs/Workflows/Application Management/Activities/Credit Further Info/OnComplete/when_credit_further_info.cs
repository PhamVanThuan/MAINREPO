using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Credit_Further_Info.OnComplete
{
    [Subject("Activity => Credit_Further_Info => OnComplete")] // AutoGenerated
    internal class when_credit_further_info : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Credit_Further_Info(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}