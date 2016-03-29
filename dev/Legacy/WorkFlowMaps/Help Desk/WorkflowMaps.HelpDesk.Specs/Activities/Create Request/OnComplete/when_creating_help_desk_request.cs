using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.HelpDesk.Specs.Activities.Create_Request.OnComplete
{
    [Subject("Activity => Create_Request => OnComplete")]
    internal class when_creating_help_desk_request : WorkflowSpecHelpDesk
    {
        private static ICommon commonClient;
        private static string expectedName;
        private static string Name;
        private static string instanceDataName;
        private static string message;
        private static bool result;

        private Establish context = () =>
            {
                message = string.Empty;
                expectedName = "Test";
                instanceDataName = string.Format(@"HD: {0}", expectedName);
                workflowData.LegalEntityKey = 1;

                ((InstanceDataStub)instanceData).CreatorADUserName = @"SAHL\ClintonS";

                commonClient = An<ICommon>();
                commonClient.WhenToldTo(x => x.GetLegalEntityLegalName((IDomainMessageCollection)messages, workflowData.LegalEntityKey, SAHL.Common.BusinessModel.Interfaces.LegalNameFormat.Full))
                    .Return(expectedName);

                domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Create_Request(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_set_the_current_consultant_data_property_to_the_instance_data_creator_adusername_property = () =>
            {
                workflowData.CurrentConsultant.ShouldBeTheSameAs(instanceData.CreatorADUserName);
            };

        private It should_set_the_instance_data_subject_property = () =>
            {
                instanceData.Subject.ShouldBeTheSameAs(expectedName);
            };

        private It should_set_instance_data_name_property = () =>
            {
                instanceData.Name = instanceDataName;
            };

        private It should_retrieve_the_legalentity_legalname = () =>
            {
                commonClient.WasToldTo(x => x.GetLegalEntityLegalName((IDomainMessageCollection)messages, workflowData.LegalEntityKey, SAHL.Common.BusinessModel.Interfaces.LegalNameFormat.Full));
            };
    }
}