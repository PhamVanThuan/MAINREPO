using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.IsResub.OnStart
{
    [Subject("Activity => IsResub => OnStart")]
    internal class when_is_resub : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.IsResub = true;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_IsResub(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}