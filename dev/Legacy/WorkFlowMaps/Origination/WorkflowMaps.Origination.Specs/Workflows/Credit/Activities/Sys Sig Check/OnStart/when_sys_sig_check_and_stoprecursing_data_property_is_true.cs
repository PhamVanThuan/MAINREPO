using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Sys_Sig_Check.OnStart
{
    [Subject("Activity => Sys_Sig_Check => OnStart")]
    internal class when_sys_sig_check_and_stoprecursing_data_property_is_true : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            workflowData.StopRecursing = true;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Sys_Sig_Check(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}