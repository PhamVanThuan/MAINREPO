using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing.Providers;
using SAHL.Core.X2.Exceptions;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Node;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using System;
using System.Collections.Generic;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.X2WorkflowRequestHandlerSpecs
{
    public class when_handling_a_system_request_group_of_3_where_the_second_throws_an_exception : WithFakes
	{
		private static StructureMap.AutoMocking.AutoMocker<X2WorkflowRequestHandler> autoMocker;
		private static long instanceId = 12;
		private static string userName = "userName";
		private static Activity activity1;
		private static Activity activity2;
		private static Activity activity3;
        private static HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand command1;
        private static HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand command2;
        private static HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand command3;
		private static Exception mapReturnedFalseException;
		private static X2SystemRequestGroup request;
		private static List<string> activityNames = new List<string>();
        private static List<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand> commands;
        static IX2ServiceCommandRouter commandHandler;
        static ServiceRequestMetadata metadata;

		private Establish context = () =>
		{
            commandHandler = An<IX2ServiceCommandRouter>();
            
			activity1 = new Activity(1, "one", 1, "state1", 2, "state2", 1, false);
			activity2 = new Activity(1, "two", 1, "state1", 3, "state3", 1, false);
			activity3 = new Activity(1, "three", 1, "state1", 4, "state4", 1, false);
			command1 = new HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand(instanceId, activity1, userName);
			command2 = new HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand(instanceId, activity2, userName);
			command3 = new HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand(instanceId, activity3, userName);
			autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2WorkflowRequestHandler>();
            commands = new List<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand> { command1, command2, command3 };
			autoMocker.Get<ICommandFactory>().WhenToldTo(x => x.CreateCommands(Param.IsAny<IX2Request>())).Return(commands);

			commandHandler.WhenToldTo(x => x.HandleCommand<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand>(Arg.Is<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand>(y => y.Activity.ActivityName == "one"),metadata)).Throw(new MapReturnedFalseException(SystemMessageCollection.Empty()));
            commandHandler.WhenToldTo(x => x.HandleCommand<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand>(Arg.Is<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand>(y => y.Activity.ActivityName == "two"), metadata)).Throw(new Exception("something bad"));
            commandHandler.WhenToldTo<IX2ServiceCommandRouter>(x => x.HandleCommand<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand>(Arg.Is<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand>(y => y.Activity.ActivityName == "two"), metadata)).Callback<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand>(command =>
			{
				command.Result = true;
			});
            autoMocker.Get<IIocContainer>().WhenToldTo(x => x.GetInstance<IX2ServiceCommandRouter>()).Return(commandHandler);
			request = new X2SystemRequestGroup(Guid.NewGuid(), metadata, X2RequestType.SystemRequestGroup, instanceId, activityNames);

            MockRepositoryProvider.GetReadWriteRepository();
		};

		private Because of = () =>
		{
			autoMocker.ClassUnderTest.Handle(request);
		};

		private It should_get_the_commands_to_execute_from_the_command_handler = () =>
		{
			Catch.Exception(()=>autoMocker.Get<ICommandFactory>().WasToldTo(x => x.CreateCommands(request)));
		};

		private It should_execute_the_first_command = () =>
		{
            commandHandler.WasToldTo(x => x.HandleCommand(Arg.Is<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand>(y => y.Activity.ActivityName == "one"), metadata));
		};

		private It should_execute_the_second_command = () =>
		{
            commandHandler.WasToldTo(x => x.HandleCommand(Arg.Is<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand>(y => y.Activity.ActivityName == "two"), metadata));
		};

		private It should_not_execute_the_third_command = () =>
		{
            commandHandler.WasNotToldTo(x => x.HandleCommand(Arg.Is<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand>(y => y.Activity.ActivityName == "three"), metadata));
		};

        private It should_create_an_error_response = () =>
        {
            autoMocker.Get<IX2ResponseFactory>().WasToldTo(x => x.CreateErrorResponse(request, Arg.Any<string>(), Arg.Any<long?>(), Arg.Any<SystemMessageCollection>()));
        };

        private It should_not_delete_all_the_scheduled_activities_for_the_request = () =>
        {
            foreach (var command in commands)
            {
                commandHandler.WasNotToldTo(x => x.HandleCommand<DeleteScheduledActivityCommand>(Arg.Is<DeleteScheduledActivityCommand>(y => y.ActivityId == command.Activity.ActivityID && y.InstanceId == command.InstanceId), metadata));
            }
        };
	}
}