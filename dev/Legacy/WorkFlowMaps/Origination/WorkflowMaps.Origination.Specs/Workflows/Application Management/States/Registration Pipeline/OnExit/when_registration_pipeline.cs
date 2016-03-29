using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Registration_Pipeline.OnExit
{
    [Subject("State => Registration_Pipeline => OnExit")]
    internal class when_registration_pipeline : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Registration_Pipeline(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_state_property = () =>
        {
            workflowData.PreviousState.ShouldMatch("Registration Pipeline");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}