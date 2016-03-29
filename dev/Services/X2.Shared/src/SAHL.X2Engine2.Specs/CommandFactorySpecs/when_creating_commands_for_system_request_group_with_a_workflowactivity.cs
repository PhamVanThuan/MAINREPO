using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandFactorySpecs
{
    public class when_creating_commands_for_system_request_group_with_a_workflowactivity : WithFakes
    {
        private static AutoMocker<CommandFactory> automocker = new NSubstituteAutoMocker<CommandFactory>();
        private static IX2Request request;
        private static List<string> activityNames = new List<string>();
        private static WorkFlowActivityDataModel workFlowActivityDataModel;
        private static long instanceId = 12;
        private static IEnumerable<IServiceCommand> commands;
        private static WorkFlowDataModel workflow1;
        private static WorkFlowDataModel workflow2;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            workFlowActivityDataModel = new WorkFlowActivityDataModel(1, "WorkflowActivity", 2, 101, 10, 3);
            workflow1 = new WorkFlowDataModel(1, 1, null, "workflow1", DateTime.Now, "", "", 1, "", null);
            workflow2 = new WorkFlowDataModel(2, 1, null, "workflow2", DateTime.Now, "", "", 1, "", null);
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            request = new X2WorkflowRequest(Guid.NewGuid(), serviceRequestMetadata, X2RequestType.WorkflowActivity, instanceId, 1, false);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowActivityDataModelById(1)).Return(workFlowActivityDataModel);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(1)).Return(workflow1);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(2)).Return(workflow2);
        };

        private Because of = () =>
        {
            commands = automocker.ClassUnderTest.CreateCommands(request);
        };

        private It should_return_a_list_with_one_command_that_is_a_systemrequestwithnosplitnoworkflowactivities = () =>
        {
            HandleSystemRequestWithNoSplitWithWorkflowActivitiesCommand handleSystemRequestWithNoSplitWithWorkflowActivitiesCommand = commands.First() as HandleSystemRequestWithNoSplitWithWorkflowActivitiesCommand;
            handleSystemRequestWithNoSplitWithWorkflowActivitiesCommand.InstanceId.ShouldEqual(instanceId);
            handleSystemRequestWithNoSplitWithWorkflowActivitiesCommand.WorkFlowActivity.ActivityName.ShouldEqual(workFlowActivityDataModel.Name);
        };
    }
}