using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Opt_Out.OnComplete
{
    [Subject("Activity => Opt_Out => OnComplete")]
    internal class when_opt_out : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Opt_Out(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}