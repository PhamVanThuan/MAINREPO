using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Reassign_User.OnGetStageTransition
{
    [Subject("Activity => Reassign_User => OnGetStageTransition")]
    internal class when_reassign_user_and_params_data_is_not_null : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
            ((ParamsDataStub)paramsData).Data = "DataTest";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Reassign_User(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_params_data_property = () =>
        {
            result.ShouldEqual(paramsData.Data.ToString());
        };
    }
}