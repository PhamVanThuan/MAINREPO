using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.IsResubReturn.OnStart
{
    [Subject("Activity => IsResubReturn => OnStart")]
    internal class when_is_resub_return : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.IsResub = true;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_IsResubReturn(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldEqual(workflowData.IsResub);
        };
    }
}