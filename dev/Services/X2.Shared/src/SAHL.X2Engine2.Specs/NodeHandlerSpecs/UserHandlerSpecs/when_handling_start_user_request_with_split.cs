using System;
using System.Collections.Generic;
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

namespace SAHL.X2Engine2.Specs.NodeHandlerSpecs.UserHandlerSpecs
{
    public class when_handling_start_user_request_with_split : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<UserRequestStartActivityWithSplitCommandHandler> autoMocker;

        private static UserRequestStartActivityWithSplitCommand userRequestStartActivityWithSplitCommand;
        private static long instanceID;
        private static InstanceDataModel instance;
        private static InstanceDataModel newlyCreatedInstance;
        private static IX2ContextualDataProvider contextualData;
        private static Dictionary<string, string> mapVariables;
        private static IX2Map x2Map;
        private static IX2Process process;
        private static Activity activity;
        static WorkFlowDataModel workflow;
        static IServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<UserRequestStartActivityWithSplitCommandHandler>();

            x2Map = An<IX2Map>();
            process = An<IX2Process>();
            instanceID = 1;
            metadata = new ServiceRequestMetadata(new Dictionary<string, string>() 
                            {
                                { SAHL.Core.Services.ServiceRequestMetadata.HEADER_USERNAME, "userName" }
                            });
            instance = new InstanceDataModel(instanceID, 1, null, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            newlyCreatedInstance = new InstanceDataModel(instance.ID + 1, 1, instance.ID, "name", "subject", "workflowProvider", 1, "creator", DateTime.Now, null, null, null, null, null, null, null, null);
            contextualData = An<IX2ContextualDataProvider>();
            activity = new Activity(1, "activityName", 1, "State", 2, "State2", 1, false);
            userRequestStartActivityWithSplitCommand = new UserRequestStartActivityWithSplitCommand(instanceID, "username", activity, mapVariables, "workflowProviderName", false, null);
            autoMocker.Get<IX2ServiceCommandRouter>().WhenToldTo<IX2ServiceCommandRouter>(x => x.HandleCommand(Arg.Is<CreateChildInstanceCommand>(command => command.Instance == instance && command.Activity.ActivityName == userRequestStartActivityWithSplitCommand.Activity.ActivityName), metadata)).Callback<CreateChildInstanceCommand>((command) =>
            {
                command.CreatedInstance = newlyCreatedInstance;
            });
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(instance.ID)).Return(instance);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(newlyCreatedInstance.ID)).Return(newlyCreatedInstance);
            autoMocker.Get<IX2ProcessProvider>().WhenToldTo(x => x.GetProcessForInstance(newlyCreatedInstance.ID)).Return(process);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowName(newlyCreatedInstance)).Return("workflowname");
            x2Map.WhenToldTo(x => x.GetContextualData(Param.IsAny<long>())).Return(contextualData);
            process.WhenToldTo(x => x.GetWorkflowMap("workflowname")).Return(x2Map);
            workflow = new WorkFlowDataModel(1, 1, null, "workflow", DateTime.Now, "storageTable", "storageKey", 1, "", null);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(Param.IsAny<int>())).Return(workflow);

            autoMocker.Get<IX2ServiceCommandRouter>().WhenToldTo<IX2ServiceCommandRouter>(x => x.HandleCommand(Arg.Is<HandleMapReturnCommand>(Param.IsAny<HandleMapReturnCommand>()), metadata)).Callback<HandleMapReturnCommand>(y => { y.Result = true; });
            autoMocker.Get<IX2ServiceCommandRouter>().WhenToldTo<IX2ServiceCommandRouter>(x => x.HandleCommand(Arg.Is<CheckInstanceIsNotLockedCommand>(Param.IsAny<CheckInstanceIsNotLockedCommand>()), metadata)).Callback<CheckInstanceIsNotLockedCommand>(y => { y.Result = true; });
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(userRequestStartActivityWithSplitCommand, metadata);
        };

        private It should_get_the_instance = () =>
        {
            autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetInstanceDataModel(instanceID));
        };

        It should_check_that_the_instance_is_not_locked = () =>
            {
                autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<CheckInstanceIsNotLockedCommand>(c => c.InstanceId == instance.ID), metadata));
            };

        private It should_check_activity_is_valid_for_current_state = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.CheckRuleCommand(
                Arg.Is<CheckActivityIsValidForStateCommand>(command => command.Instance.StateID == instance.StateID), metadata));
        };

        It should_check_the_result_of_the_map_call = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Param.IsAny<HandleMapReturnCommand>(), metadata)).Times(1);
        };

        private It should_create_child_instance = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<CreateChildInstanceCommand>(command => command.Instance == instance && command.Activity.ActivityName == userRequestStartActivityWithSplitCommand.Activity.ActivityName), metadata));
        };

        private It should_lock_the_newly_created_instance = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<LockInstanceCommand>(command => command.Instance.ID == newlyCreatedInstance.ID), metadata));
        };

        private It should_get_the_map_for_the_newly_created_instance = () =>
        {
            process.WasToldTo(x => x.GetWorkflowMap(Param.IsAny<string>()));
        };

        private It should_load_contextual_data_for_instance = () =>
        {
            x2Map.WasToldTo(x => x.GetContextualData(Param.IsAny<long>()));
        };

        private It should_set_contextual_data = () =>
        {
            contextualData.WasToldTo(x => x.SetMapVariables(mapVariables));
        };

        private It should_call_on_start_activity = () =>
        {
            x2Map.WasToldTo(x => x.StartActivity(newlyCreatedInstance, contextualData, Arg.Any<IX2Params>(), Arg.Any<ISystemMessageCollection>()));
        };

        private It should_save_contextual_data = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<SaveContextualDataCommand>(command => command.ContextualData == contextualData), metadata));
        };

        private It should_save_the_newly_created_instance = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<SaveInstanceCommand>(command => command.Instance == newlyCreatedInstance), metadata));
        };
    }
}