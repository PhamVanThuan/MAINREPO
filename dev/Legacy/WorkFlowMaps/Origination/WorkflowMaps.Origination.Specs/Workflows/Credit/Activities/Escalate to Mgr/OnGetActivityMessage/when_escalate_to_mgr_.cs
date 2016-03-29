using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Escalate_to_Mgr.OnGetActivityMessage
{
    [Subject("Activity => Escalate_To_Mgr => OnGetActivityMessage")]
    internal class when_escalate_to_mgr_ : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abdc"; ;
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Escalate_to_Mgr_(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_params_data_property_as_string = () =>
        {
            result.ShouldBeEmpty();
        };
    }
}