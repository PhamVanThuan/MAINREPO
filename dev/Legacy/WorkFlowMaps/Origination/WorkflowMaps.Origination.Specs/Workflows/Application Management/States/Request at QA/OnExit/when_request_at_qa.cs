using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Request_at_QA.OnExit
{
    [Subject("State => Request_at_QA => OnExit")]
    internal class when_request_at_qa : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Request_at_QA(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_state_property = () =>
        {
            workflowData.PreviousState.ShouldMatch("Request at QA");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}