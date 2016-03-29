using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Further_Info_Request.OnExit
{
    [Subject("State => Further_Info_Request => OnExit")]
    internal class when_further_info_request : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Further_Info_Request(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_state_property = () =>
        {
            workflowData.PreviousState.ShouldMatch("Further Info Request");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}