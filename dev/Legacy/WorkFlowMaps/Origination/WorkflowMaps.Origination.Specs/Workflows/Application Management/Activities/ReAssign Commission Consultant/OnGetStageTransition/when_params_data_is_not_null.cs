using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.ReAssign_Commission_Consultant.OnGetStageTransition
{
    [Subject("Activity => ReAssign_Commission_Consultant => OnGetStageTransition")]
    internal class when_reassign_commision_consultant_and_params_data_is_not_null : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
            ((ParamsDataStub)paramsData).Data = "DataTest";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_ReAssign_Commission_Consultant(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_params_data_property = () =>
        {
            result.ShouldEqual(paramsData.Data.ToString());
        };
    }
}