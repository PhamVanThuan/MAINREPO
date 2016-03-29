using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Reload_CaseName.OnComplete
{
    [Subject("Activity => Reload_CaseName => OnComplete")]
    internal class when_reload_case_name : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static string expectedSubject;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            expectedSubject = "SubjectTest";
            ((InstanceDataStub)instanceData).Subject = string.Empty;
            workflowData.ApplicationKey = 1;
            common = An<ICommon>();
            common.WhenToldTo(x => x.GetCaseName(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()))
                .Return(expectedSubject);
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Reload_CaseName(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_get_case_name = () =>
        {
            common.WasToldTo(x => x.GetCaseName((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_set_instance_data_subject_property_to_case_name = () =>
        {
            instanceData.Subject.ShouldEqual(expectedSubject);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}