using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Assigned_QA.OnGetStageTransition
{
    [Subject("Activity => Assigned_QA => OnGetStageTransition")] // AutoGenerated
    internal class when_assigned_qa : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Assigned_QA(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}