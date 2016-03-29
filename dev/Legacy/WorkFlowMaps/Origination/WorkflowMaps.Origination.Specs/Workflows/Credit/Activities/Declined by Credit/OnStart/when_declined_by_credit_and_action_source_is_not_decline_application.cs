using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Declined_by_Credit.OnStart
{
    [Subject("Activity => Declined_By_Credit => OnStart")]
    internal class when_declined_by_credit_and_action_source_is_not_decline_application : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            workflowData.ActionSource = "Dispute Indicated";
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Declined_by_Credit(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}