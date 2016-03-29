using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Agree_with_decision.OnGetStageTransition
{
    [Subject("Activity => Agree_With_Decision => OnGetStageTransition")]
    internal class when_agree_with_decision : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Agree_with_decision(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_agree_with_descision = () =>
        {
            result.ShouldBeTheSameAs("Agree with decision");
        };
    }
}