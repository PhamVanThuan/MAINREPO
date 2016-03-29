using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Resubmission.OnStart
{
    [Subject("Activity => Resubmission => OnStart")]
    internal class when_resubmission_and_is_not_a_resub : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            workflowData.IsResub = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Resubmission(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}