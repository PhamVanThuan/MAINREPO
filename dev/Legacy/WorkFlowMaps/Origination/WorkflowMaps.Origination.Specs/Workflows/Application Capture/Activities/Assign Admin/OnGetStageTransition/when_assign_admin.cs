using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Assign_Admin.OnGetStageTransition
{
    [Subject("Activity => Assign_Admin => OnGetStageTransition")]
    internal class when_assign_admin : WorkflowSpecApplicationCapture
    {
        private static string expectedResult;
        private static string result;

        private Establish context = () =>
        {
            expectedResult = "Assign Admin";
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Assign_Admin(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_assign_admin_stage_transition = () =>
        {
            result.ShouldBeTheSameAs(expectedResult);
        };
    }
}