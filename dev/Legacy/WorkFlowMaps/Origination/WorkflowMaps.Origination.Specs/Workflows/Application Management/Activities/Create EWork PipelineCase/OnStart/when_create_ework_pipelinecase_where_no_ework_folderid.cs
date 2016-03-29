using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Create_EWork_PipelineCase.OnStart
{
    [Subject("Activity => Create_EWork_PipelineCase => OnStart")]
    internal class when_create_ework_pipelinecase_where_no_ework_folderid : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static bool expectedResult;
        private static string expectEWorkFolderID;
        private static IApplicationManagement appMan;
        private static ICommon common;
        private static int callCount;

        private Establish context = () =>
        {
            result = false;
            expectedResult = true;
            workflowData.EWorkFolderID = string.Empty;
            string eworkFolderID = string.Empty;
            expectEWorkFolderID = "Test";
            callCount = 0;

            appMan = An<IApplicationManagement>();
            appMan.Expect(x => x.CreateEWorkPipelineCase((IDomainMessageCollection)messages, workflowData.ApplicationKey, out eworkFolderID))
                .OutRef(expectEWorkFolderID)
                .Return(expectedResult)
                .WhenCalled((y) => { callCount++; });
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Create_EWork_PipelineCase(instanceData, workflowData, paramsData, messages);
        };

        private It should_create_ework_pipeline_case = () =>
        {
            callCount.ShouldEqual(1);
        };

        private It should_set_ework_folder_id_data_property = () =>
        {
            workflowData.EWorkFolderID.ShouldMatch(expectEWorkFolderID);
        };

        private It should_return_create_ework_pipeline_case_result = () =>
        {
            result.ShouldEqual(expectedResult);
        };
    }
}