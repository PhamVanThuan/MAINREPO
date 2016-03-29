using Machine.Specifications;

namespace WorkflowMaps.HelpDesk.Specs.Roles
{
    internal class when_getting_current_consultant : WorkflowSpecHelpDesk
    {
        private static string currentConsultant;
        private static string result;

        private Establish context = () =>
        {
            currentConsultant = @"SAHL\Clintons";
            workflowData.CurrentConsultant = currentConsultant;
        };

        private Because of = () =>
        {
            result = workflow.OnGetRole_Help_Desk_Current_Consultant(instanceData, workflowData, currentConsultant, paramsData, messages);
        };

        private It should_return_the_current_consultant_data_property = () =>
        {
            result.ShouldBeTheSameAs(workflowData.CurrentConsultant);
        };
    }
}