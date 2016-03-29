using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Issue_AIP.OnExit
{
    [Subject("State => Issue_AIP => OnExit")]
    internal class when_issue_aip : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Issue_AIP(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_state_property = () =>
        {
            workflowData.PreviousState.ShouldMatch("Issue AIP");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}