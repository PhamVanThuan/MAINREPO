using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Refer_Senior_Analyst.OnGetActivityMessage
{
    [Subject("Activity => Refer_Senior_Analyst => OnGetActivityMessage")]
    internal class when_refer_senior_analyst : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Refer_Senior_Analyst(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}