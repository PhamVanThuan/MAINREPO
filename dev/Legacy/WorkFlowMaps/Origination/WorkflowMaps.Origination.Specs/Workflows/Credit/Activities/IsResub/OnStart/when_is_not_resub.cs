using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.IsResub.OnStart
{
    [Subject("Activity => IsResub => OnStart")]
    internal class when_is_not_resub : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            workflowData.IsResub = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_IsResub(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}