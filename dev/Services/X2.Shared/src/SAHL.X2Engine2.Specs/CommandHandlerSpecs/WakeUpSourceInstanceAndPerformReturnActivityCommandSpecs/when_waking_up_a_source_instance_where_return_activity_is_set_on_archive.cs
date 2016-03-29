using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.WakeUpSourceInstanceAndPerformReturnActivityCommandSpecs
{
    public class when_waking_up_a_source_instance_where_return_activity_is_set_on_archive : WithFakes
    {
        private static AutoMocker<WakeUpSourceInstanceAndPerformReturnActivityCommandHandler> automocker = new NSubstituteAutoMocker<WakeUpSourceInstanceAndPerformReturnActivityCommandHandler>();
        private static WakeUpSourceInstanceAndPerformReturnActivityCommand command;
        private static InstanceDataModel remoteInstance;
        private static InstanceDataModel sourceInstance;
        private static Activity activity;
        private static ActivityDataModel returnActivity;
        private static StateDataModel archiveState;
        private static IX2Process process;
        private static IX2Map x2Map;
        private static IX2ContextualDataProvider contextualData;
        static WorkFlowDataModel workflow;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
            {
                x2Map = An<IX2Map>();
                process = An<IX2Process>();
                contextualData = An<IX2ContextualDataProvider>();
                activity = new Activity(10, "Archive", 10, "PreArchive", 11, "Archive", 2, false);
                returnActivity = new ActivityDataModel(20, 0, "ReturnActivity", 4, null, 101, false, 9, null, "activityMessage", null, null, null, null, null, Guid.NewGuid());
                archiveState = new StateDataModel(11, 0, "Archive", 5, false, null, 0, 20, Guid.NewGuid());
                remoteInstance = new InstanceDataModel(12, 1, null, "remoteName", "subject", "WFDP", 56, "creator", DateTime.Now, DateTime.Now, null, null, null, null, 9, 11, 9);
                sourceInstance = new InstanceDataModel(11, 0, null, "sourceName", "subject", "WFDP", 100, "creator", DateTime.Now, DateTime.Now, null, null, null, null, 9, null, null);

                x2Map.WhenToldTo(x => x.OnReturnState(remoteInstance, contextualData, Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>())).Return(true);
                automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel((long)remoteInstance.SourceInstanceID)).Return(sourceInstance);
                automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetStateById((int)remoteInstance.StateID)).Return(archiveState);
                automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivity((int)archiveState.ReturnActivityID)).Return(returnActivity);
                command = new WakeUpSourceInstanceAndPerformReturnActivityCommand(remoteInstance, activity, x2Map, contextualData);
                workflow = new WorkFlowDataModel(1, 1, null, "workflow", DateTime.Now, "storageTable", "storageKey", 1, "", null);
                automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(Param.IsAny<int>())).Return(workflow);
            };

        private Because of = () =>
            {
                automocker.ClassUnderTest.HandleCommand(command, metadata);
            };

        private It should_call_onreturnstate_on_the_map = () =>
            {
                x2Map.WasToldTo(x => x.OnReturnState(Arg.Is<InstanceDataModel>(i => i.ID == remoteInstance.ID), Arg.Any<IX2ContextualDataProvider>(), Arg.Any<IX2Params>(), Param.IsAny<ISystemMessageCollection>()));
            };

        private It should_get_the_source_instance = () =>
            {
                automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetInstanceDataModel((long)remoteInstance.SourceInstanceID));
            };

        private It should_get_the_state_the_remote_instance_is_transitioning_to = () =>
            {
                automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetStateById((int)remoteInstance.StateID));
            };

        private It should_get_the_return_activity = () =>
            {
                automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetActivity((int)archiveState.ReturnActivityID));
            };

        private It should_send_the_system_request_group_tp_the_engine = () =>
            {
                automocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.QueueUpCommandToBeProcessed(Arg.Is<PublishBundledRequestCommand>(c =>
                    ((NotificationOfNewSystemRequestGroupCommand)c.Commands[0]).InstanceId == command.Instance.SourceInstanceID)));
            };
    }
}