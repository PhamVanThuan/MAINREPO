using Machine.Specifications;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Opt_Out_Not_Required.OnComplete
{
    [Subject("Activity => Opt_Out_Not_Required => OnComplete")]
    internal class when_opt_out_not_required : WorkflowSpecDebtCounselling
    {
        private static IDebtCounselling client;
        private static bool result;
        private static string message;

        private Establish context = () =>
            {
                result = false;
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Opt_Out_Not_Required(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };
    }
}