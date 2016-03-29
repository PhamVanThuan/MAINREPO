using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Reassign_User.OnGetStageTransition
{
    [Subject("Activity => Reassign_User => OnGetStageTransition")]
    internal class when_reasign_user_and_params_data_is_null : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
            ((ParamsDataStub)paramsData).Data = null;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Reassign_User(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_empty_string = () =>
        {
            result.ShouldBeEmpty();
        };
    }
}