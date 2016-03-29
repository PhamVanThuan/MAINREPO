using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.FL_Rework_Application.OnGetStageTransition
{
    [Subject("Activity => FL_Rework_Application => OnGetStageTransition")]
    internal class when_fl_rework_application : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_FL_Rework_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_fl_rework_application = () =>
        {
            result.ShouldMatch("FL Rework Application");
        };
    }
}