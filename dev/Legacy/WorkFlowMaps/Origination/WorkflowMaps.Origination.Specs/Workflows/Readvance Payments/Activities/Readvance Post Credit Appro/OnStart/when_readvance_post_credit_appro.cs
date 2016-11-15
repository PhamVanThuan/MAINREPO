using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities.Readvance_Post_Credit_Appro.OnStart
{
    [Subject("Activity => Readvance_Post_Credit_Appro => OnStart")] // AutoGenerated
    internal class when_readvance_post_credit_appro : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Readvance_Post_Credit_Appro(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}