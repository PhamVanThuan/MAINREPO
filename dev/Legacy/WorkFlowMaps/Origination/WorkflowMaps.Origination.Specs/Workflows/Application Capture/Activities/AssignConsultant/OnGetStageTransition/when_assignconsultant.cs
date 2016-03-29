using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.AssignConsultant.OnGetStageTransition
{
    [Subject("Activity => AssignConsultant => OnGetStageTransition")]
    internal class when_assignconsultant : WorkflowSpecApplicationCapture
    {
        private static string expectedResult;
        private static string result;

        private Establish context = () =>
        {
            expectedResult = "AssignConsultant";
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_AssignConsultant(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_assign_consultant_stage_transition = () =>
        {
            result.ShouldBeTheSameAs(expectedResult);
        };
    }
}