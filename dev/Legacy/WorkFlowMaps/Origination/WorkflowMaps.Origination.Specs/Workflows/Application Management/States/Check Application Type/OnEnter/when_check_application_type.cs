using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Check_Application_Type.OnEnter
{
    [Subject("State => Check_Application_Type => OnEnter")] // AutoGenerated
    internal class when_check_application_type : WorkflowSpecApplicationManagement
    {
        static bool result;
        Establish context = () =>
        {
            result = false;
        };
		
        Because of = () =>
        {
            result = workflow.OnEnter_Check_Application_Type(instanceData, workflowData, paramsData, messages);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}