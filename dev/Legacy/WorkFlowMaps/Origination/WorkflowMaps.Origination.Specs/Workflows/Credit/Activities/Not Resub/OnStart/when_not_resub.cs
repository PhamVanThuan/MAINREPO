using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Not_Resub.OnStart
{
    [Subject("Activity => Not_Resub => OnStart")]
    internal class when_not_resub : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.IsResub = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Not_Resub(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldEqual(!workflowData.IsResub);
        };
    }
}