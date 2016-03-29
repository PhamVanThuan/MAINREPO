using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.X2.Events;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.RecordStageTransitionCommandHandlerSpecs
{
    public class when_recording_a_stage_transition_for_non_legacy : WithFakes
    {
        private static AutoMocker<RecordStageTransitionCommandHandler> automocker;
        private static RecordStageTransitionCommand command;
        private static string stageTransitionComments = "Comments";
        private static InstanceDataModel instance;
        private static IX2ContextualDataProvider contextualDataProvider;
        private static string userName = "bob";
        private static Activity activity;
        private static StageActivityDataModel stageActivityDataModel;
        private static IReadWriteSqlRepository readWriteSqlRepository;
        private static ADUserDataModel aduser;
        private static WorkFlowDataModel workFlowDataModel;
        private static IServiceRequestMetadata metadata;
        private static ProcessDataModel nonLegacyProcessDataModel;
        private static string processName;
        private static int genericKey;
        private static string workflowName;
        private static DateTime activityDate;
        private static string workflowActivity;
        private static string fromStateName;
        private static string toStateName;
        private static int genericKeyTypeKey;

        private Establish context = () =>
        {
            metadata = An<IServiceRequestMetadata>();
            processName = "processName";
            genericKey = 12;
            workflowName = "workflowName";
            genericKeyTypeKey = 1001;
            activityDate = DateTime.Now;
            workflowActivity = "activity";
            fromStateName = "stateFrom";
            toStateName = "stateTo";
            automocker = new NSubstituteAutoMocker<RecordStageTransitionCommandHandler>();
            nonLegacyProcessDataModel = new ProcessDataModel(1, processName, "version", null, activityDate, null, null, null, false);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetProcessById(Param.IsAny<int>())).Return(nonLegacyProcessDataModel);
            workFlowDataModel = new WorkFlowDataModel(1, null, workflowName, DateTime.Now, "storageTable", "storageKey", 1, "default subject", genericKeyTypeKey);
            aduser = new ADUserDataModel(userName, 1, null, null, null, null);
            readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
            activity = new Activity(1, "activity", 1, fromStateName, 2, toStateName, 1, false);
            stageActivityDataModel = new StageActivityDataModel(1, 1, 3456, 2345);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetStageActivities(activity.ActivityID)).Return(new List<StageActivityDataModel> { stageActivityDataModel });
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetADUser(userName)).Return(aduser);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflow(Param.IsAny<InstanceDataModel>())).Return(workFlowDataModel);
            contextualDataProvider = An<IX2ContextualDataProvider>();
            contextualDataProvider.WhenToldTo(x => x.GetDataField(Param.IsAny<string>())).Return(genericKey.ToString());
            instance = new InstanceDataModel(1, 1, null, "name", "subject", "provider", 10, "creator", DateTime.Now, null, null, null, null, null, null, null, null);
            command = new RecordStageTransitionCommand(instance, stageTransitionComments, contextualDataProvider, userName, activity, activityDate);
        };

        private Because of = () =>
        {
            automocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        private It should_get_stage_activity_rows = () =>
        {
            automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetStageActivities(activity.ActivityID));
        };

        private It should_get_the_aduser_record_for_the_username = () =>
        {
            automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetADUser(userName));
        };

        private It should_not_create_a_stagetransition = () =>
        {
            readWriteSqlRepository.WasNotToldTo(x => x.Insert<StageTransitionDataModel>(Arg.Is<StageTransitionDataModel>(c => c.StageDefinitionStageDefinitionGroupKey == 2345)));
        };

        private It should_raise_a_stagetransition_event = () =>
        {
            automocker.Get<IEventRaiser>().WasToldTo(ev => ev.RaiseEvent(
                  Param<DateTime>.Matches(m => m.Equals(command.StartTime))
                , Param<IEvent>
                   .Matches(m => ((WorkflowTransitionEvent)m).GenericKey == genericKey
                              && ((WorkflowTransitionEvent)m).ProcessName.Equals(processName, StringComparison.Ordinal)
                              && ((WorkflowTransitionEvent)m).WorkflowName.Equals(workflowName, StringComparison.Ordinal)
                              && ((WorkflowTransitionEvent)m).WorkflowActivity.Equals(workflowActivity, StringComparison.Ordinal)
                              && ((WorkflowTransitionEvent)m).FromStateName.Equals(fromStateName, StringComparison.Ordinal)
                              && ((WorkflowTransitionEvent)m).ToStateName.Equals(toStateName, StringComparison.Ordinal)
                              && ((WorkflowTransitionEvent)m).ActivityDate == activityDate
                )
                , genericKey
                , genericKeyTypeKey
                , Param.IsAny<IServiceRequestMetadata>()
            ));
        };

        private It should_not_create_an_xml_result = () =>
        {
            command.XmlResult.ShouldBeNull();
        };
    }
}