using Machine.Specifications;
using WorkflowMaps.Credit.Specs;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Escalate_to_Mgr.OnGetStageTransition
{
    [Subject("Activity => Escalate_To_Mgr => OnGetStageTransition")]
    internal class when_escalate_to_mgr_and_params_data_is_null : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
            ((ParamsDataStub)paramsData).Data = null;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Escalate_To_Exceptions_Mgr(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeEmpty();
        };
    }
}