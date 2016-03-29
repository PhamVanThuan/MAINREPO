using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Override_Decision.OnGetStageTransition
{
    [Subject("Activity => Override_Decision => OnGetStageTransition")]
    internal class when_override_decision : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Override_Decision(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_override_decision = () =>
        {
            result.ShouldBeTheSameAs("Override Decision");
        };
    }
}