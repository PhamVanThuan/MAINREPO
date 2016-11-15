using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.States.Require_SuperLo_Opt_Out.OnExit
{
    [Subject("State => Require_SuperLo_Opt_Out => OnExit")] // AutoGenerated
    internal class when_require_superlo_opt_out : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Require_SuperLo_Opt_Out(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}