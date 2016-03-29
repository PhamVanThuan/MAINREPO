using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Opt_Out.OnStart
{
    [Subject("Activity => Opt_Out => OnStart")]
    internal class when_opt_out : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Opt_Out(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}