using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Create_EWork_PipelineCase.OnStart
{
    [Subject("Activity => Create_EWork_PipelineCase => OnStart")]
    internal class when_create_ework_pipelinecase_where_ework_forderid_exists : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static bool expectedResult;
        private static string expectEWorkFolderID;
        private static IApplicationManagement appMan;
        private static ICommon common;

        private Establish context = () =>
        {
            result = true;
            expectedResult = false;
            workflowData.EWorkFolderID = "Test";
            expectEWorkFolderID = workflowData.EWorkFolderID;

            appMan = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);

            common = An<ICommon>();
            common.WhenToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>()))
                .Return(expectedResult);
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Create_EWork_PipelineCase(instanceData, workflowData, paramsData, messages);
        };

        private It should_perform_ework_action = () =>
        {
            common.WasToldTo(x => x.PerformEWorkAction((IDomainMessageCollection)messages, expectEWorkFolderID,
                SAHL.Common.Constants.EworkActionNames.X2REINSTRUCTED,
                workflowData.ApplicationKey,
                paramsData.ADUserName,
                paramsData.StateName));
        };

        private It should_return_perform_ework_action_result = () =>
        {
            result.ShouldEqual(expectedResult);
        };
    }
}