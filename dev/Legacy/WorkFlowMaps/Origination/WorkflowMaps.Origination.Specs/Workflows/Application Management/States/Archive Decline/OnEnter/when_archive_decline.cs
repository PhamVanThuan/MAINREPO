using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Archive_Decline.OnEnter
{
    [Subject("State => Archive_Decline => OnEnter")] // AutoGenerated
    internal class when_archive_decline : WorkflowSpecApplicationManagement
    {
        static bool result;
        Establish context = () =>
        {
            result = true;
        };
		
        Because of = () =>
        {
            result = workflow.OnEnter_Archive_Decline(messages, workflowData, instanceData, paramsData);
        };

        It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}