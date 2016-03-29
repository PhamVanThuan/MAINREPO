using Machine.Specifications;
using System;
using WorkflowMaps.Valuations.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Manager_Archive.OnGetStageTransition
{
    [Subject("Activity => Manager_Archive => OnGetStageTransition")]
    internal class when_manager_archive : WorkflowSpecValuations
    {
        private static string result;

        private Establish context = () =>
        {
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Manager_Archive(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_instruct_ez_val_valuer_stagetransition = () =>
        {
            result.ShouldEqual<string>("Manager Archive");
        };
    }
}