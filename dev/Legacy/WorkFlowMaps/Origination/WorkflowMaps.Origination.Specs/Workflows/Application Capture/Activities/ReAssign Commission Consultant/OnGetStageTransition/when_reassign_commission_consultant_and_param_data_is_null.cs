using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.ReAssign_Commission_Consultant.OnGetStageTransition
{
    [Subject("Activity => ReAssign_Commission_Consultant => OnGetStageTransition")]
    internal class when_reassign_commission_consultant_and_param_data_is_null : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = "test";
            ((ParamsDataStub)paramsData).Data = null;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_ReAssign_Commission_Consultant(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeEmpty();
        };
    }
}