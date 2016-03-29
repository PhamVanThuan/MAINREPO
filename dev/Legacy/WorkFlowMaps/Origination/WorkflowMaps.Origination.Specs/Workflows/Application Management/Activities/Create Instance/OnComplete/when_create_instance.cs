using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Create_Instance.OnComplete
{
    [Subject("Activity => Create_Instance => OnComplete")]
    internal class when_create_instance : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static ICommon common;
        private static string expectedSubject;
        private static int expectedOfferTypekey;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            instanceData.Subject = string.Empty;
            expectedSubject = "Test";
            workflowData.IsResub = true;
            workflowData.OfferTypeKey = 0;
            expectedOfferTypekey = 1;

            common = An<ICommon>();
            common.WhenToldTo(x => x.GetCaseName(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()))
                .Return(expectedSubject);
            common.WhenToldTo(x => x.GetApplicationType(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()))
                .Return(expectedOfferTypekey);
            domainServiceLoader.RegisterMockForType<ICommon>(common);

            var appMan = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Create_Instance(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_instance_data_subject_property_to_case_name = () =>
        {
            instanceData.Subject.ShouldMatch(expectedSubject);
        };

        private It should_set_is_resub_data_property_to_false = () =>
        {
            workflowData.IsResub.ShouldBeFalse();
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