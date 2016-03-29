using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core;
using SAHL.Core.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Node;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;


namespace SAHL.X2Engine2.Specs.X2WorkflowRequestHandlerSpecs
{
    public class when_handling_a_user_complete_request : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2WorkflowRequestHandler> autoMocker;
        private static IX2ServiceCommandRouter commandHandler;
        private static X2Request request;
        private static X2Response response;
        private static UserRequestCompleteActivityCommand command;
        private static IServiceRequestMetadata serviceRequestMetadata;

        Establish context = () =>
        {
            ViewModels.Activity activity = new ViewModels.Activity(1, "Activity One", 12, "State", 1, "Created", 1, false);
            command = new UserRequestCompleteActivityCommand(1234657L, activity, @"SAHL\HaloUser", false, new Dictionary<string, string>());
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2WorkflowRequestHandler>();
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            request = new X2Request(Guid.NewGuid(), X2RequestType.UserCreate, null, false, serviceRequestMetadata);
            commandHandler = An<IX2ServiceCommandRouter>();
            autoMocker.Get<ICommandFactory>().WhenToldTo(x => x.CreateCommands(Param.IsAny<IX2Request>())).Return(new List<IServiceCommand> { command });
            autoMocker.Get<IIocContainer>().WhenToldTo(x => x.GetInstance<IX2ServiceCommandRouter>()).Return(commandHandler);
            MockRepositoryProvider.GetReadWriteRepository();
        };

        Because of = () =>
        {
            response = autoMocker.ClassUnderTest.Handle(request);
        };

        It should_process_the_queued_commands = () =>
        {
            commandHandler.WasToldTo(x => x.ProcessQueuedCommands(serviceRequestMetadata));
        };
    }
}