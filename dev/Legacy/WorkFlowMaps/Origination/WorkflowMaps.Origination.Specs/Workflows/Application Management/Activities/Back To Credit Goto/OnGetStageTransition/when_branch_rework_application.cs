using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Back_To_Credit_Goto.OnGetStageTransition
{
    [Subject("Activity => Back_To_Credit_Goto => OnGetStageTransition")]
    internal class when_branch_rework_application : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abc";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Branch_Rework_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_empty_string = () =>
        {
            result.ShouldBeEmpty();
        };
    }
}