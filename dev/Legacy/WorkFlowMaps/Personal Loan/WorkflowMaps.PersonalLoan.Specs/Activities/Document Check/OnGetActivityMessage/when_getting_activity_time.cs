using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Document_Check.OnGetActivityMessage
{
    [Subject("Activity => Document_Check => OnGetActivityMessage")]
    public class when_getting_activity_time : WorkflowSpecPersonalLoans
    {
        private static string result;

        private Establish context = () =>
        {
            result = "asdf";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Document_Check(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldEqual(string.Empty);
        };
    }
}