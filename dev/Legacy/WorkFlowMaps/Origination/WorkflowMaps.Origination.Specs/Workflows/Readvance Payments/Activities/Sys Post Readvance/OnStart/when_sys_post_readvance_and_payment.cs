using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Sys_Post_Readvance.OnStart
{
    [Subject("State => Sys_Post_Readvance => OnStart")]
    internal class when_sys_post_readvance_and_payment : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.EntryPath = 1;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Sys_Post_Readvance(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}