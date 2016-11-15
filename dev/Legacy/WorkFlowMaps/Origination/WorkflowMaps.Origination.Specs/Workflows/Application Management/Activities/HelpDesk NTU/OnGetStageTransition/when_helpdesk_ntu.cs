using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.HelpDesk_NTU.OnGetStageTransition
{
    [Subject("Activity => HelpDesk_NTU => OnGetStageTransition")] // AutoGenerated
    internal class when_helpdesk_ntu : WorkflowSpecApplicationManagement
    {
        static string result;
        Establish context = () =>
        {
            result = "abcd";
        };

        Because of = () =>
        {
            result = workflow.GetStageTransition_HelpDesk_NTU(instanceData, workflowData, paramsData, messages);
        };

        It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}