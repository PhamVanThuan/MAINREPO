using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Specs.X2WorkflowRequestHandlerSpecs
{
    public class when_handling_a_user_create_with_complete_request : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2WorkflowRequestHandler> autoMocker;
        private static IX2ServiceCommandRouter commandHandler;
        private static X2Request request;
        private static X2Response response;
        private static UserRequestCreateInstanceWithCompleteCommand createCommand;
        private static IServiceRequestMetadata serviceRequestMetadata;


        Establish context = () =>
            {
                ViewModels.Activity activity = new ViewModels.Activity(1, "Create", null, string.Empty, 1, "Created", 1, false);
                createCommand = new UserRequestCreateInstanceWithCompleteCommand("Process", "Workflow", "Username", activity, new Dictionary<string, string>(), "test", false);
                createCommand.NewlyCreatedInstanceId = 1234567L;
                autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2WorkflowRequestHandler>();
                serviceRequestMetadata = new ServiceRequestMetadata(new Dictionary<string, string>() 
                            {
                                { ServiceRequestMetadata.HEADER_USERNAME, "X2" }
                            });
                request = new X2Request(Guid.NewGuid(), X2RequestType.UserCreateWithComplete, null, false, serviceRequestMetadata);
                commandHandler = An<IX2ServiceCommandRouter>();
                autoMocker.Get<ICommandFactory>().WhenToldTo(x => x.CreateCommands(Param.IsAny<IX2Request>())).Return(new List<IServiceCommand> { createCommand });
                autoMocker.Get<IIocContainer>().WhenToldTo(x => x.GetInstance<IX2ServiceCommandRouter>()).Return(commandHandler);
                autoMocker.Get<IX2ResponseFactory>().WhenToldTo(x => x.CreateSuccessResponse(request, Param.IsAny<long>(), Param.IsAny<ISystemMessageCollection>())).Return(new X2Response(request.CorrelationId, string.Empty, createCommand.NewlyCreatedInstanceId));
                MockRepositoryProvider.GetReadWriteRepository();

            };

        Because of = () =>
            {
                response = autoMocker.ClassUnderTest.Handle(request);
            };

        It should_handle_the_create_instance_with_complete_command = () =>
        {
            commandHandler.WasToldTo(x => x.HandleCommand(createCommand, serviceRequestMetadata));
        };

        It should_return_a_success_response = () =>
        {
            response.ShouldBeOfExactType(typeof(X2Response));
        };

        It should_return_the_newly_created_instance_in_the_response = () =>
        {
            autoMocker.Get<IX2ResponseFactory>().WasToldTo(x => x.CreateSuccessResponse(request, createCommand.NewlyCreatedInstanceId, Param.IsAny<ISystemMessageCollection>()));
        };


    }
}
