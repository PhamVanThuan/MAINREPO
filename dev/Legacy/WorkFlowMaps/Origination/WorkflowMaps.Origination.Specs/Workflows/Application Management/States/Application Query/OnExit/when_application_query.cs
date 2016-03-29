using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Application_Query.OnExit
{
    [Subject("State => Application_Query => OnExit")]
    internal class when_application_query : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Application_Query(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_previous_state_property = () =>
       {
           workflowData.PreviousState.ShouldMatch("Application Query");
       };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}