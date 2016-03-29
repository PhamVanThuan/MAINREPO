using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Declined_by_Credit.OnStart
{
    [Subject("Activity => Declined_By_Credit => OnStart")]
    internal class when_declined_by_credit : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.ActionSource = "Decline Application";
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Declined_by_Credit(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}