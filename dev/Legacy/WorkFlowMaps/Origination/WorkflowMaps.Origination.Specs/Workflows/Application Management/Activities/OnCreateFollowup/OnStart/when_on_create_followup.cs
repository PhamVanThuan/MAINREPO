using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.OnCreateFollowup.OnStart
{
    [Subject("Activity => OnCreateFollowup => OnStart")]
    internal class when_on_create_followup : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string expectedSubject;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            expectedSubject = "SubjectTest";
            workflowData.ApplicationKey = 1;
            ((InstanceDataStub)instanceData).Subject = string.Empty;
            ((InstanceDataStub)instanceData).Name = string.Empty;
            common = An<ICommon>();
            common.WhenToldTo(x => x.GetCaseName(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()))
                .Return(expectedSubject);
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_OnCreateFollowup(instanceData, workflowData, paramsData, messages);
        };

        private It should_get_case_name = () =>
        {
            common.WasToldTo(x => x.GetCaseName((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_set_instance_data_subject_parameter_to_case_name = () =>
        {
            instanceData.Subject.ShouldEqual(expectedSubject);
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