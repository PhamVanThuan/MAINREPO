using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Check_Source_and_LeadType.OnEnter
{
    [Subject("State => Check_Source_and_LeadType => OnEnter")]
    internal class when_check_source_and_leadtype : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            instanceData.Name = "test";
            workflowData.IsEA = true;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Check_Source_and_LeadType(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_name_property = () =>
        {
            instanceData.Name.ShouldMatch(workflowData.ApplicationKey.ToString());
        };

        private It should_set_is_ea_property = () =>
        {
            workflowData.IsEA.ShouldBeFalse();
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}