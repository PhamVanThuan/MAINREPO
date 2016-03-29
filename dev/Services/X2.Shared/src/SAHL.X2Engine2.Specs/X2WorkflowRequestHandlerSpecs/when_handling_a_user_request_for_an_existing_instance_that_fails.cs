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
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Node;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;


namespace SAHL.X2Engine2.Specs.X2WorkflowRequestHandlerSpecs
{
    public class when_handling_a_user_request_for_an_existing_instance_that_fails : WithFakes
    {
        private static X2WorkflowRequestHandler autoMocker;
        private static IServiceCommand command;
        private static X2RequestForExistingInstance request;
        private static X2Response response;
        static X2Response errorResponse;
        static IX2ResponseFactory responseFactory;
        static ICommandFactory commandFactory;
        static IX2ServiceCommandRouter commandHandler;
        static IIocContainer iocContainer;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            iocContainer = An<IIocContainer>();
            command = An<IServiceCommand>();
            responseFactory = An<IX2ResponseFactory>();
            commandFactory = An<ICommandFactory>();
            commandHandler = An<IX2ServiceCommandRouter>();
            iocContainer.WhenToldTo(x => x.GetInstance<IX2ServiceCommandRouter>()).Return(commandHandler);
            commandFactory.WhenToldTo(x => x.CreateCommands(Param.IsAny<IX2Request>())).Return(new List<IServiceCommand> { command });
            serviceRequestMetadata = new ServiceRequestMetadata(new Dictionary<string, string>() 
                            {
                                { ServiceRequestMetadata.HEADER_USERNAME, "X2" }
                            });
            request = new X2RequestForExistingInstance(Guid.NewGuid(), 12, X2RequestType.UserCancel, serviceRequestMetadata, "activityName", false);
            commandHandler.WhenToldTo(x => x.HandleCommand(command, serviceRequestMetadata)).Throw(new Exception());
            errorResponse = new X2ErrorResponse(request.CorrelationId, "", request.InstanceId, new SystemMessageCollection());
            responseFactory.WhenToldTo(x => x.CreateErrorResponse(Param.IsAny<IX2Request>(), Param.IsAny<string>(), Param.IsAny<long>(), Param.IsAny<SystemMessageCollection>())).Return(errorResponse);
            autoMocker = new X2WorkflowRequestHandler(commandFactory, responseFactory, iocContainer);

            MockRepositoryProvider.GetReadWriteRepository();
        };

        private Because of = () =>
        {
            response = autoMocker.Handle(request);
        };

        private It should_get_the_commands_to_execute_from_the_command_handler = () =>
        {
            commandFactory.WasToldTo(x => x.CreateCommands(request));
        };

        private It should_execute_the_commands = () =>
        {
            commandHandler.WasToldTo(x => x.HandleCommand(Arg.Any<IServiceCommand>(), serviceRequestMetadata));
        };

        private It should_return_an_error_response = () =>
        {
            response.ShouldBeOfExactType(typeof(X2ErrorResponse));
        };
    }
}