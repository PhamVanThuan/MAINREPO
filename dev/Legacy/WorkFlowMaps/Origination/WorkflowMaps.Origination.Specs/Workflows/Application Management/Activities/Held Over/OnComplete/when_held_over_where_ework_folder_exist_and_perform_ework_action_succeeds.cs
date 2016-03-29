using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Held_Over.OnComplete
{
    [Subject("Activity => Held_Over => OnComplete")]
    internal class when_held_over_where_ework_folder_exist_and_perform_ework_action_succeeds : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.EWorkFolderID = "EWorkFolderIDTest";
            workflowData.ApplicationKey = 1;
            ((ParamsDataStub)paramsData).ADUserName = "ADUserNameTest";
            ((ParamsDataStub)paramsData).StateName = "StateNameTest";
            common = An<ICommon>();
            common.WhenToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<string>(),
                Param.IsAny<string>(),
                Param.IsAny<int>(),
                Param.IsAny<string>(),
                Param.IsAny<string>()))
                .Return(true);
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Held_Over(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_perform_ework_action = () =>
        {
            common.WasToldTo(x => x.PerformEWorkAction((IDomainMessageCollection)messages,
                workflowData.EWorkFolderID,
                SAHL.Common.Constants.EworkActionNames.X2HOLDOVER,
                workflowData.ApplicationKey,
                paramsData.ADUserName,
                paramsData.StateName));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}