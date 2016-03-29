using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandFactorySpecs
{
    public class when_creating_command_for_a_timer_request : WithFakes
    {
        private static AutoMocker<CommandFactory> automocker = new NSubstituteAutoMocker<CommandFactory>();
        private static IX2SystemRequest systemRequest;

        private static Activity activity { get; set; }

        private static long instanceId = 12;
        private static string activityName = "timer";
        private static IEnumerable<IServiceCommand> commands;
        private static IWorkflowDataProvider workflowDataProvider;
        private static IServiceRequestMetadata serviceRequestMetadata;

        Establish context = () =>
        {
            activity = new Activity(1, "timer", 1, "stateWithTimer", 2, "archiveState", 1, false);
            workflowDataProvider = automocker.Get<IWorkflowDataProvider>();
            workflowDataProvider.WhenToldTo(x => x.GetActivityForInstanceAndName(instanceId, activityName)).Return(activity);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivityForInstanceAndName(instanceId, activityName)).Return(activity);
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            systemRequest = new X2SystemRequestGroup(Guid.NewGuid(), serviceRequestMetadata, X2RequestType.Timer, instanceId, new List<string>() { activity.ActivityName });
        };

        Because of = () =>
        {
            commands = automocker.ClassUnderTest.CreateCommands(systemRequest);
        };

        It should_get_the_activity_given_instanceId_and_name = () =>
        {
            workflowDataProvider.WasToldTo(x => x.GetActivityForInstanceAndName(Param.Is<long>(instanceId), Param.Is<string>(activityName)));
        };

        It should_return_command_for_timer_request = () =>
        {
            commands.Count().ShouldBeGreaterThan(0);
            HandleSystemRequestBaseCommand command = commands.FirstOrDefault() as HandleSystemRequestBaseCommand;
            command.Activity.ActivityName.ShouldEqual(activityName);
            command.UserName.ShouldEqual(serviceRequestMetadata.UserName);
            command.InstanceId.ShouldEqual(instanceId);
        };
    }
}