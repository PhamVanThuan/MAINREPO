using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Disbursement_Review.OnExit
{
    [Subject("State => Disbursement_Review => OnExit")]
    internal class when_disbursement_review : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Disbursement_Review(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_state_property = () =>
        {
            workflowData.PreviousState.ShouldMatch("Disbursement Review");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}