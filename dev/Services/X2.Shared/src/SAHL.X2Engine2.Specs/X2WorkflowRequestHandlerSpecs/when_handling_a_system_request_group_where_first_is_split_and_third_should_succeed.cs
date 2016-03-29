using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Exceptions;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Node;

using SAHL.X2Engine2.ViewModels;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.X2WorkflowRequestHandlerSpecs
{
    public class when_handling_a_system_request_group_where_first_is_split_and_third_should_succeed : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2WorkflowRequestHandler> autoMocker;
        private static long instanceId = 12;
        private static string userName = "userName";
        private static Activity activity1;
        private static Activity activity2;
        private static Activity activity3;
        private static IServiceCommand command1;
        private static IServiceCommand command2;
        private static IServiceCommand command3;
        private static X2SystemRequestGroup request;
        private static List<string> activityNames = new List<string>();
        static IIocContainer iocContainer;
        static IX2ServiceCommandRouter commandHandler;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            iocContainer = An<IIocContainer>();
            commandHandler = An<IX2ServiceCommandRouter>();
            activity1 = new Activity(1, "one", 1, "state1", 2, "state2", 1, true);
            activity2 = new Activity(1, "two", 1, "state1", 3, "state3", 1, false);
            activity3 = new Activity(1, "three", 1, "state1", 4, "state4", 1, false);
            command1 = new HandleSystemRequestWithSplitWithNoWorkflowActivitiesCommand(instanceId, activity1, userName, "");
            command2 = new HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand(instanceId, activity2, userName);
            command3 = new HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand(instanceId, activity3, userName);
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2WorkflowRequestHandler>();
            autoMocker.Get<ICommandFactory>().WhenToldTo(x => x.CreateCommands(Param.IsAny<IX2Request>())).Return(new List<IServiceCommand> { command1, command2, command3 });

            commandHandler
                .WhenToldTo(x => x.HandleCommand<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand>(Arg.Is<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand>(y =>
                       y.Activity.ActivityName == "two"), serviceRequestMetadata))
                .Throw(new MapReturnedFalseException(SystemMessageCollection.Empty()));

            autoMocker.Get<IIocContainer>().WhenToldTo(x => x.GetInstance<IX2ServiceCommandRouter>()).Return(commandHandler);
            serviceRequestMetadata = new ServiceRequestMetadata(new Dictionary<string, string>() 
                            {
                                { ServiceRequestMetadata.HEADER_USERNAME, "X2" }
                            });
            request = new X2SystemRequestGroup(Guid.NewGuid(), serviceRequestMetadata, X2RequestType.SystemRequestGroup, instanceId, activityNames);

            MockRepositoryProvider.GetReadWriteRepository();
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.Handle(request);
        };

        private It should_get_the_commands_to_execute_from_the_command_handler = () =>
        {
            autoMocker.Get<ICommandFactory>().WasToldTo(x => x.CreateCommands(request));
        };

        private It should_execute_the_first_command = () =>
        {
            commandHandler.WasToldTo(x => x.HandleCommand(Arg.Is<HandleSystemRequestWithSplitWithNoWorkflowActivitiesCommand>(y => y.Activity.ActivityName == "one"), serviceRequestMetadata));
        };

        private It should_execute_the_second_command = () =>
        {
            commandHandler.WasToldTo(x => x.HandleCommand(Arg.Is<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand>(y => y.Activity.ActivityName == "two"), serviceRequestMetadata));
        };

        private It should_execute_the_third_command = () =>
        {
            commandHandler.WasToldTo(x => x.HandleCommand(Arg.Is<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand>(y => y.Activity.ActivityName == "three"), serviceRequestMetadata));
        };
    }
}