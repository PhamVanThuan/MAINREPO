using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Create_Followup.OnComplete
{
    [Subject("Activity => Create_Followup => OnComplete")]
    internal class when_create_followup : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static string expectedSubject;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            instanceData.Subject = string.Empty;
            expectedSubject = "Test";
            instanceData.Name = string.Empty;
            workflowData.ApplicationKey = 1;

            common = An<ICommon>();
            common.WhenToldTo(x => x.GetCaseName(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()))
                .Return(expectedSubject);
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Create_Followup(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_instance_data_subject_property_to_case_name = () =>
        {
            instanceData.Subject.ShouldMatch(expectedSubject);
        };

        private It should_set_instance_data_name_property_to_application_key = () =>
        {
            instanceData.Name.ShouldMatch(workflowData.ApplicationKey.ToString());
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}