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
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.NodeHandlerSpecs.UserHandlerSpecs
{
    public class when_handling_create_activity_request : WithFakes
    {
        private static AutoMocker<UserRequestCreateInstanceCommandHandler> autoMocker;
        private static UserRequestCreateInstanceCommand userRequestCreateInstanceCommand;
        private static InstanceDataModel newlyCreatedInstance;
        private static Activity activity;
        private static IX2Process process;
        private static IX2Map x2Map;
        private static IX2ContextualDataProvider contextualData;
        private static Dictionary<string, string> mapVariables;
        private static string userName;
        private static string workflowName;
        private static string processName;
        static WorkFlowDataModel workflow;
        static IServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            autoMocker = new NSubstituteAutoMocker<UserRequestCreateInstanceCommandHandler>();
            x2Map = An<IX2Map>();
            mapVariables = new Dictionary<string, string>();
            metadata = new ServiceRequestMetadata(new Dictionary<string, string>() 
                            {
                                { SAHL.Core.Services.ServiceRequestMetadata.HEADER_USERNAME, "userName" }
                            });
            newlyCreatedInstance = new InstanceDataModel(9, 1, null, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            activity = new Activity(-1, "activityName", -1, "stateName", -1, "toStateName", 1, false);
            userName = "username";
            workflowName = "workflowName";
            processName = "processName";
            process = An<IX2Process>();

            userRequestCreateInstanceCommand = new UserRequestCreateInstanceCommand(processName, workflowName, userName, activity, mapVariables, "", false);

            autoMocker.Get<IX2ProcessProvider>().WhenToldTo(x => x.GetProcessForInstance(newlyCreatedInstance.ID)).Return(process);
            process.WhenToldTo(x => x.GetWorkflowMap(Param.IsAny<string>())).Return(x2Map);

            contextualData = An<IX2ContextualDataProvider>();

            autoMocker.Get<IX2ServiceCommandRouter>().WhenToldTo<IX2ServiceCommandRouter>(x => x.HandleCommand(Arg.Is<CreateInstanceCommand>(command => command.ProcessName == processName && command.WorkflowName == workflowName), metadata)).Callback<CreateInstanceCommand>((command) =>
            {
                command.NewlyCreatedInstanceId = newlyCreatedInstance.ID;
            });

            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(newlyCreatedInstance.ID)).Return(newlyCreatedInstance);

            x2Map.WhenToldTo(x => x.GetContextualData(Param.IsAny<long>())).Return(contextualData);
            workflow = new WorkFlowDataModel(1, 1, null, "workflow", DateTime.Now, "storageTable", "storageKey", 1, "", null);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(Param.IsAny<int>())).Return(workflow);

            autoMocker.Get<IX2ServiceCommandRouter>().WhenToldTo<IX2ServiceCommandRouter>(x => x.HandleCommand(Arg.Is<HandleMapReturnCommand>(Param.IsAny<HandleMapReturnCommand>()), metadata)).Callback<HandleMapReturnCommand>(y => { y.Result = true; });
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(userRequestCreateInstanceCommand, metadata);
        };

        private It should_check_the_activity_exists_and_is_a_create_activity = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.CheckRuleCommand(
                Arg.Is<CheckCreationActivityAccessCommand>(c => c.Activity == userRequestCreateInstanceCommand.Activity), metadata));
        };

        private It should_check_activity_security_does_not_have_dynamic_roles = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.CheckRuleCommand(
                Arg.Is<CheckCreateActivityHasOnlyStaticRoleCommand>(c => c.Activity == activity && c.UserName == userName), metadata));
        };

        It should_check_the_result_of_the_map_call = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Param.IsAny<HandleMapReturnCommand>(), metadata)).Times(1);
        };

        private It should_create_the_instance = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<CreateInstanceCommand>(c => c.WorkflowName == workflowName && c.ProcessName == processName), metadata));
        };

        private It should_get_the_map_for_the_newly_created_instance = () =>
        {
            autoMocker.Get<IX2ProcessProvider>().WasToldTo(x => x.GetProcessForInstance(newlyCreatedInstance.ID));
            process.WasToldTo(x => x.GetWorkflowMap(Param.IsAny<string>()));
        };

        private It should_insert_contextual_data = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<CreateContextualDataCommand>(c => c.Instance == newlyCreatedInstance && c.MapVariables == mapVariables), metadata));
        };

        private It should_load_contextual_data_for_instance = () =>
        {
            x2Map.WasToldTo(x => x.GetContextualData(Param.IsAny<long>()));
        };

        private It should_call_on_start_activity = () =>
        {
            x2Map.WasToldTo(x => x.StartActivity(newlyCreatedInstance, contextualData, Arg.Any<IX2Params>(), Arg.Any<ISystemMessageCollection>()));
        };

        private It should_save_contextual_data = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<SaveContextualDataCommand>(c => c.ContextualData == contextualData), metadata));
        };

        private It should_save_the_newly_created_instance = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<SaveInstanceCommand>(c => c.Instance == newlyCreatedInstance), metadata));
        };
    }
}