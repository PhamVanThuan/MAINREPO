using System;
using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.EXT_Create_Capitec_Instance.OnGetStageTransition
{
    [Subject("Activity => EXT_Create_Capitec_Instance => OnGetStageTransition")]
    internal class when_ext_create_capitec_instance : WorkflowSpecApplicationCapture
    {
        private static string expectedResult;
        private static string result;

        private Establish context = () =>
        {
            expectedResult = "Create Capitec Instance";
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_EXT_Create_Capitec_Instance(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_create_capitec_instance_stage_transition = () =>
        {
            result.ShouldBeTheSameAs(expectedResult);
        };
    }
}