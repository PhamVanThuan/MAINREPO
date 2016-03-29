using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Awaiting_Application.OnEnter
{
    [Subject("States => Awaiting_Application => OnEnter")]
    internal class when_awaiting_application : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string expectedName;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            workflowData.IsFL = false;
            instanceData.Name = "Test";
            expectedName = "Test";
            workflowData.IsResub = true;
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            common.WhenToldTo(x => x.GetCaseName((IDomainMessageCollection)messages, workflowData.ApplicationKey)).Return(expectedName);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Awaiting_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_workflow_data_is_fl_property = () =>
        {
            workflowData.IsFL.ShouldBeTrue();
        };

        private It should_get_case_name = () =>
        {
            common.WasToldTo(x => x.GetCaseName((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_set_instance_data_subject_property = () =>
        {
            instanceData.Subject.ShouldMatch(expectedName);
        };

        private It should_set_instance_data_name_property = () =>
        {
            instanceData.Name.ShouldMatch(workflowData.ApplicationKey.ToString());
        };

        private It should_set_workflow_data_is_resub_property = () =>
        {
            workflowData.IsResub.ShouldBeFalse();
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}