using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Reinstate_Decline.OnStart
{
    [Subject("Activity => Reinstate_Decline => OnStart")]
    internal class when_reinstate_decline : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.ApplicationKey = 1;
            ((InstanceDataStub)instanceData).Name = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Reinstate_Decline(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_instance_data_name_parameter_to_application_key = () =>
        {
            instanceData.Name.ShouldEqual(workflowData.ApplicationKey.ToString());
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}