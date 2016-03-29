using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Request_Further_Info.OnStart
{
    [Subject("Activity => Request_Further_Info => OnStart")]
    internal class when_request_further_info : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.ActionSource = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Request_Further_Info(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}