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
    public class when_creating_commands_for_user_create_request : WithFakes
    {
        private static AutoMocker<CommandFactory> automocker = new NSubstituteAutoMocker<CommandFactory>();
        private static IX2Request request;
        private static IEnumerable<IServiceCommand> commands;
        private static string activityName = "Create";
        private static string workflowName = "Workflow";
        private static Activity activity;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            request = new X2CreateInstanceRequest(Guid.NewGuid(), activityName, "Process", workflowName, serviceRequestMetadata, false);

            activity = new Activity(1, "Create", 0, "", 1, "Created", 1, false);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivityByNameAndWorkflowName(activityName, workflowName)).Return(activity);
        };

        private Because of = () =>
        {
            commands = automocker.ClassUnderTest.CreateCommands(request);
        };

        private It should_load_the_activity_for_create_activity = () =>
            {
                automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetActivityByNameAndWorkflowName(activityName, workflowName));
            };

        private It return_a_user_create_instance_command = () =>
        {
            commands.First().ShouldBe(typeof(UserRequestCreateInstanceCommand));
        };
    }
}