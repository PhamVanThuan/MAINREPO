using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Readvance_Post_Credit_Appro.OnComplete
{
    [Subject("Activity => Readvance_Post_Credit_Appro => OnComplete")] // AutoGenerated
    internal class when_readvance_post_credit_appro : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Readvance_Post_Credit_Appro(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}