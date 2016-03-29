using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Sys_Appro_FL_and_FA.OnStart
{
    [Subject("State => Sys_Appro_FL_And_FA => OnStart")]
    internal class when_sys_appro_FL_and_FA_and_payments_entry_path_is_3 : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.EntryPath = 3;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Sys_Appro_FL_and_FA(instanceData, workflowData, paramsData, messages);
        };

        private It should_ = () =>
        {
            result.ShouldBeTrue();
        };
    }
}