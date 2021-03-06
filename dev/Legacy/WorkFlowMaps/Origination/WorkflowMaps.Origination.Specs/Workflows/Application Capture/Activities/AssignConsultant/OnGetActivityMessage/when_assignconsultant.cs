using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.AssignConsultant.OnGetActivityMessage
{
    [Subject("Activity => AssignConsultant => OnGetActivityMessage")] // AutoGenerated
    internal class when_assignconsultant : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_AssignConsultant(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}