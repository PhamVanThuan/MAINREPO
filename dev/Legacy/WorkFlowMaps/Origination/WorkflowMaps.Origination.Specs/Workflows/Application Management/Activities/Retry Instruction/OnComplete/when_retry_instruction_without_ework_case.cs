using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Retry_Instruction.OnComplete
{
    [Subject("Activity => Retry_Instruction => OnComplete")]
    internal class when_retry_instruction_without_ework_case : WorkflowSpecApplicationManagement
    {
        private static IApplicationManagement applicationManagementClient;
        private static ICommon commonClient;
        private static string expectedEworkFolderId;
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            expectedEworkFolderId = "1234567890123456";

            applicationManagementClient = An<IApplicationManagement>();
            commonClient = An<ICommon>();

            applicationManagementClient.Expect(x =>
                    x.CreateEWorkPipelineCase((IDomainMessageCollection)messages, workflowData.ApplicationKey,
                            out expectedEworkFolderId)).OutRef(expectedEworkFolderId).Return(true);
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(applicationManagementClient);
            domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Retry_Instruction(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_add_instruction_send_detail_type = () =>
        {
            applicationManagementClient.WasToldTo(x => x.AddDetailTypeInstructionSent((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_create_ework_pipeline_case = () =>
        {
            applicationManagementClient.AssertWasCalled(x => x.CreateEWorkPipelineCase((IDomainMessageCollection)messages,
                                                    workflowData.ApplicationKey, out expectedEworkFolderId));
        };

        private It should_set_eworkfolderid_data_property = () =>
        {
            workflowData.EWorkFolderID.ShouldEqual<string>(expectedEworkFolderId);
        };

        private It should_return_what_create_ework_pipeline_case_return = () =>
        {
            //mocked above to return true
            result.ShouldEqual<bool>(true);
        };
    }
}