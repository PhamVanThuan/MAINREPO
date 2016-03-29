using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Proceed_with_Application.OnComplete
{
    [Subject("Activity => Proceed_with_Application => OnComplete")]
    internal class when_proceed_with_application : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static string expectedSubject;
        private static int expectedOfferTypekey;
        private static ICommon common;
        private static IApplicationManagement appMan;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            expectedSubject = "SubjectTest";
            expectedOfferTypekey = 1;
            workflowData.ApplicationKey = 2;
            ((InstanceDataStub)instanceData).Subject = string.Empty;
            ((InstanceDataStub)instanceData).Name = string.Empty;
            workflowData.IsResub = true;
            common = An<ICommon>();
            common.WhenToldTo(x => x.GetCaseName(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()))
                .Return(expectedSubject);
            common.WhenToldTo(x => x.GetApplicationType(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()))
                .Return(expectedOfferTypekey);
            domainServiceLoader.RegisterMockForType<ICommon>(common);

            appMan = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Proceed_with_Application(instanceData, workflowData, paramsData, messages, ref message);
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

        private It should_set_is_resub_data_property_to_false = () =>
        {
            workflowData.IsResub.ShouldBeFalse();
        };

        private It should_get_offer_type = () =>
        {
            common.WasToldTo(x => x.GetApplicationType((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_set_offer_type_key_data_property = () =>
        {
            workflowData.OfferTypeKey.ShouldEqual(expectedOfferTypekey);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}