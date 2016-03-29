using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Create_Instance.OnGetStageTransition
{
    [Subject("Activity => Create_Instance => OnGetStageTransition")]
    internal class when_create_instance : WorkflowSpecApplicationCapture
    {
        private static string expectedResult;
        private static string result;

        private Establish context = () =>
        {
            expectedResult = "Create Instance";
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Create_Instance(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_create_instance_transition = () =>
        {
            result.ShouldBeTheSameAs(expectedResult);
        };
    }
}