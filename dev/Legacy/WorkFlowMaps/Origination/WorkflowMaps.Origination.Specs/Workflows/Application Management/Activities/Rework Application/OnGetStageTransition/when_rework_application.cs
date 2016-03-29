using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Rework_Application.OnGetStageTransition
{
    [Subject("Activity => Rework_Application => OnGetStageTransition")]
    internal class when_rework_application : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Rework_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_rework_application_stagetransition = () =>
        {
            result.ShouldEqual<string>("Rework Application");
        };
    }
}