using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.States.Contact_Client.OnExit
{
    [Subject("State => Contact_Client => OnExit")]
    internal class when_contact_client : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Contact_Client(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_state = () =>
        {
            workflowData.PreviousState.ShouldMatch("Contact Client");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}