using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Resub.OnStart
{
    [Subject("Activity => Resub => OnStart")]
    internal class when_resub : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.IsResub = true;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Resub(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldEqual(workflowData.IsResub);
        };
    }
}