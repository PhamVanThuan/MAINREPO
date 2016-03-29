using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Rework_Application.OnGetActivityMessage
{
    [Subject("Activity => Rework_Application => OnGetActivityMessage")]
    internal class when_rework_application : WorkflowSpecPersonalLoans
    {
        private static string result;

        private Establish context = () =>
        {
            result = "Test";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Rework_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_empty_string = () =>
        {
            result.ShouldBeEmpty();
        };
    }
}