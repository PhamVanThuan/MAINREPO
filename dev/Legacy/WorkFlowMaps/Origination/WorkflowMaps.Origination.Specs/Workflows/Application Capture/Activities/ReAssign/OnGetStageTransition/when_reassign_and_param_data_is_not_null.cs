using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.ReAssign.OnGetStageTransition
{
    [Subject("Activity => ReAssign => OnGetStageTransition")]
    internal class when_reassign_and_param_data_is_not_null : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
            ((ParamsDataStub)paramsData).Data = "Test";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_ReAssign(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_the_params_data_data_property_as_a_string = () =>
        {
            result.ShouldMatch(paramsData.Data.ToString());
        };
    }
}