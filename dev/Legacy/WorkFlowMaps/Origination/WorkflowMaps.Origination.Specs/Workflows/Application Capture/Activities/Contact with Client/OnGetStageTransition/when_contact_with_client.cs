using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Contact_with_Client.OnGetStageTransition
{
    [Subject("Activity => Contact_with_Client => OnGetStageTransition")]
    internal class when_contact_with_client : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Contact_with_Client(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_emptyr = () =>
        {
            result.ShouldEqual(string.Empty);
        };
    }
}