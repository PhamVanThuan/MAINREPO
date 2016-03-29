using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Contact_Client.OnExit
{
    [Subject("State => Contact_Client => OnExit")]
    internal class when_contact_client : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.Last_State = "test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Contact_Client(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_last_state_property = () =>
        {
            workflowData.Last_State.ShouldMatch("Contact Client");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}