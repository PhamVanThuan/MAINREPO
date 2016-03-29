using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Accept_Declaration.OnComplete
{
    [Subject("Activity => Accept_Declaration => OnComplete")]
    internal class when_accept_declaration : WorkflowSpecLife
    {
        private static bool result;
        private static string message = string.Empty;

        private Establish context = () =>
        {
            result = false;
            workflowData.DeclarationDone = 0;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Accept_Declaration(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_the_declaration_done_data_property_to_true = () =>
        {
            workflowData.DeclarationDone.ShouldEqual(1);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}