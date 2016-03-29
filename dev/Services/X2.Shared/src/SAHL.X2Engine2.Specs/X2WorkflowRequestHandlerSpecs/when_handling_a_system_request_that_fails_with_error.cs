using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Node;

using SAHL.X2Engine2.ViewModels;
using StructureMap.AutoMocking;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.X2WorkflowRequestHandlerSpecs
{
    public class when_handling_a_system_request_that_fails_with_error : WithFakes
    {
        private static AutoMocker<X2WorkflowRequestHandler> workflowRequestHandlerMock;
        private static SAHL.Core.X2.Messages.X2SystemRequestGroup systemRequestGroupThatWillFail;
        private static IX2ServiceCommandRouter commandHandler;
        private static IServiceCommand command;
        private static IServiceRequestMetadata serviceRequestMetadata;

        Establish context = () =>
        {
            var activity = new Activity(1, "one", 1, "state1", 2, "state2", 1, true);
            command = new HandleSystemRequestWithSplitWithNoWorkflowActivitiesCommand(long.MinValue, activity, "SAHL\\User", "");
            serviceRequestMetadata = new ServiceRequestMetadata(new Dictionary<string, string>() 
                            {
                                { ServiceRequestMetadata.HEADER_USERNAME, "X2" }
                            });
            systemRequestGroupThatWillFail = new Core.X2.Messages.X2SystemRequestGroup(Guid.NewGuid(), serviceRequestMetadata, Core.X2.Messages.X2RequestType.SystemRequestGroup, long.MinValue, new List<string>(new string[] { activity.ActivityName }));
            workflowRequestHandlerMock = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2WorkflowRequestHandler>();
            workflowRequestHandlerMock.Get<ICommandFactory>().WhenToldTo(x => x.CreateCommands(Param.IsAny<IX2Request>())).Return(new List<IServiceCommand> { command });

            var messages = new SystemMessageCollection();
            messages.AddMessage(new SystemMessage("warning", SystemMessageSeverityEnum.Warning));

            commandHandler = An<IX2ServiceCommandRouter>();
            workflowRequestHandlerMock.Get<IIocContainer>().WhenToldTo(x => x.GetInstance<IX2ServiceCommandRouter>()).Return(commandHandler);
            commandHandler.WhenToldTo(x => x.HandleCommand(Param.IsAny<IServiceCommand>(), serviceRequestMetadata)).Return(messages);

            MockRepositoryProvider.GetReadWriteRepository();
        };

        Because of = () =>
        {
            workflowRequestHandlerMock.ClassUnderTest.Handle(systemRequestGroupThatWillFail);
        };

        It should_unlock_the_instance = () =>
        {
            commandHandler.WasToldTo(x => x.HandleCommand<UnlockInstanceCommand>(Arg.Is<UnlockInstanceCommand>(y => y.InstanceID == systemRequestGroupThatWillFail.InstanceId), serviceRequestMetadata));
        };
    }
}