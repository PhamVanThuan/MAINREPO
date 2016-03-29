using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Escalate_To_Exceptions_Mgr.OnGetActivityMessage
{
    [Subject("Activity => Escalate_To_Exceptions_Mgr => OnGetActivityMessage")]
    internal class when_escalate_to_exceptions_mgr : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abdc";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Escalate_To_Exceptions_Mgr(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}