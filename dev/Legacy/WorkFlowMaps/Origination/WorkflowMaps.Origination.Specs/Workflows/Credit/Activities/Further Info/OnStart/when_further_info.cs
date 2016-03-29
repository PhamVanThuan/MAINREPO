using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Further_Info.OnStart
{
    [Subject("Activity => Further_Info => OnStart")]
    internal class when_further_info : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.ActionSource = "Request Further Info";
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Further_Info(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}