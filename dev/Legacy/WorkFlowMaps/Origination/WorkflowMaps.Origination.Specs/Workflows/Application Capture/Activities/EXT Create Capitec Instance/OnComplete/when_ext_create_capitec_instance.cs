using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.Common;
using Machine.Fakes;
using System;
using SAHL.Common.Collections.Interfaces;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.EXT_Create_Capitec_Instance.OnComplete
{
    [Subject("Activity => EXT_Create_Capitec_Instance => OnComplete")]
    internal class when_ext_create_capitec_instance : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static string message;
        private static int applicationKey;
        private static string expectedInstanceName, expectedCaseName;
        private static string assignedTo;
        private static ICommon common;


        private Establish context = () =>
        {
            result = false;
            instanceData.Subject = string.Empty;
            instanceData.Name = string.Empty;
            instanceData.ID = 0;

            assignedTo = @"SAHL\TELCUser";
            applicationKey = 123456;
            expectedInstanceName = Convert.ToString(applicationKey);
            expectedCaseName = "Mr Test & Mrs Test";

            common = An<ICommon>();

            domainServiceLoader.RegisterMockForType<ICommon>(common);
            
            workflowData.ApplicationKey = applicationKey;
            common.WhenToldTo(x => x.GetCaseName(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(expectedCaseName);

            message = string.Empty;
            workflowData.LeadType = -1;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_EXT_Create_Capitec_Instance(instanceData, workflowData, paramsData, messages, ref message);
        };


        private It should_set_instance_name_property_to_the_application_key = () =>
        {
            instanceData.Name.ShouldEqual<string>(expectedInstanceName);

        };
        private It should_set_instance_data_subject_property_to_case_name = () =>
        {
            instanceData.Subject.ShouldMatch(expectedCaseName);
        };


        private It should_set_lead_type_data_parameter_to_0 = () =>
        {
            workflowData.LeadType.ShouldEqual(0);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}