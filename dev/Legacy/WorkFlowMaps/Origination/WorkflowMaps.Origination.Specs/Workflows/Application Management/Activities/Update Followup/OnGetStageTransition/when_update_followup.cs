using Machine.Specifications;
using System;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Update_Followup.OnGetStageTransition
{
    [Subject("Activity => Update_Followup => OnGetStageTransition")]
    internal class when_update_followup : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Update_Followup(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_update_followup_stagetransition = () =>
        {
            result.ShouldEqual<string>("Update Followup");
        };
    }
}