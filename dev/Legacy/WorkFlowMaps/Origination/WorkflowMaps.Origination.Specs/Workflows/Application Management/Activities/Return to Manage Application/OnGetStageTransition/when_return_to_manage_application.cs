using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Return_to_Manage_Application.OnGetStageTransition
{
    [Subject("Activity => Return_to_Manage_Application => OnGetStageTransition")]
    internal class when_return_to_manage_application : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Return_to_Manage_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_manage_application_stagetransition = () =>
        {
            result.ShouldEqual<string>("Return to Manage Application");
        };
    }
}