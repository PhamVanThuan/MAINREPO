using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Create_Account_For_Application.OnGetStageTransition
{
    [Subject("Activity => Create_Account_For_Application => OnGetStageTransition")]
    internal class when_create_account_for_application : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Create_Account_For_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_create_account_for_application = () =>
        {
            result.ShouldMatch("Create Account For Application");
        };
    }
}