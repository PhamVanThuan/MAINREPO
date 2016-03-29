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

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.UpdateWorkListCommandHandlerSpecs
{
    public class when_rebuilding_the_worklist_for_a_dynamic_role_with_process_and_workflow : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<RebuildUserWorkListCommandHandler> autoMocker;

        private static RebuildUserWorkListCommand rebuildUserWorklistCommand;
        private static IX2Map map;
        private static IX2Process process;
        private static InstanceDataModel instance;
        private static string adUsername;
        private static SecurityGroupDataModel securityGroup;
        private static StateWorkListDataModel stateWorkList;
        private static StateWorkListDataModel[] stateWorkLists;
        private static IX2ContextualDataProvider contextualDataProvider;
        private static WorkFlowDataModel workflowDataModel;
        private static string activityMessage;
        static WorkFlowDataModel workflow;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<RebuildUserWorkListCommandHandler>();

            adUsername = "username";
            activityMessage = "activityMessage";

            map = An<IX2Map>();
            process = An<IX2Process>();
            contextualDataProvider = An<IX2ContextualDataProvider>();

            instance = new InstanceDataModel(9, 1, null, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(instance.ID)).Return(instance);

            securityGroup = new SecurityGroupDataModel(true, "name", "description", int.MinValue, int.MinValue);

            workflowDataModel = new WorkFlowDataModel(1, 1, null, "workflow", DateTime.Now, "storage", "offerkey", 1, "default sibject", null);

            stateWorkList = new StateWorkListDataModel(int.MinValue, int.MinValue);
            stateWorkLists = new StateWorkListDataModel[] { stateWorkList };

            workflow = new WorkFlowDataModel(1, 1, null, "workflow", DateTime.Now, "storageTable", "storageKey", 1, "", null);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(Param.IsAny<int>())).Return(workflow);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetStateWorkList(instance.StateID.Value)).Return(stateWorkLists);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetSecurityGroup(stateWorkList.SecurityGroupID)).Return(securityGroup);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflow(instance)).Return(workflowDataModel);

            autoMocker.Get<IX2ProcessProvider>().WhenToldTo(x => x.GetProcessForInstance(instance.ID)).Return(process);

            process.WhenToldTo(x => x.GetWorkflowMap(Param.IsAny<string>())).Return(map);

            map.WhenToldTo(x => x.GetContextualData(Param.IsAny<long>())).Return(contextualDataProvider);
            map.WhenToldTo(x => x.GetDynamicRole(instance, contextualDataProvider, securityGroup.Name, Arg.Any<IX2Params>(), Param.IsAny<ISystemMessageCollection>())).Return(adUsername);

            rebuildUserWorklistCommand = new RebuildUserWorkListCommand(instance.ID, activityMessage);
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(rebuildUserWorklistCommand, metadata);
        };

        private It should_clear_the_existing_worklist_for_the_instance = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<ClearWorkListCommand>(command => command.InstanceID == instance.ID), metadata));
        };

        private It should_create_a_new_worklist_entry_for_returned_ad_user_if_not_null = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Any<CreateWorkListCommand>(), metadata));
        };
    }
}