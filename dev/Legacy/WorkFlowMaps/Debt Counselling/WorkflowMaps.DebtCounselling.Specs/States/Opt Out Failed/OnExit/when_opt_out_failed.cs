using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States.Opt_Out_Failed.OnExit
{
    [Subject("Activity => Opt_Out_Failed => OnExit")]
    internal class when_opt_out_failed : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Opt_Out_Failed(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}