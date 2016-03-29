using Machine.Specifications;
using System;
using WorkflowMaps.Valuations.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Note.OnGetStageTransition
{
    [Subject("Activity => Note => OnGetStageTransition")]
    internal class when_adding_note : WorkflowSpecValuations
    {
        private static string result;

        private Establish context = () =>
        {
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Note(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_note_stagetransition = () =>
        {
            result.ShouldEqual<string>("Note");
        };
    }
}