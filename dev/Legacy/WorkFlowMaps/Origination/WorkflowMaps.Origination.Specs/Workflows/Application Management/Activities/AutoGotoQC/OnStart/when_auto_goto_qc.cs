using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.AutoGotoQC.OnStart
{
    [Subject("Activity => AutoGotoQC => OnStart")]
    internal class when_auto_goto_qc : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = true;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_AutoGotoQC(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}