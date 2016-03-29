using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Branch_Rework_Application.OnGetActivityMessage
{
    [Subject("Activity => Branch_Rework_Application => OnGetActivityMessage")] // AutoGenerated
    internal class when_branch_rework_application : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Branch_Rework_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}