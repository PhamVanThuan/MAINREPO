using Capitec.Core.Identity;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.CommandHandlers;
using SAHL.Services.Interfaces.Capitec.Commands;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.LoginCommandHandlerSpecs
{
    public class when_told_to_login : WithFakes
    {
        static IAuthenticationManager authenticationManager;
        static ISystemMessageCollection messages;
        static LoginCommand command;
        static LoginCommandHandler handler;
        static string username, password;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            username = "Bob";
            password = "carnivalclowns";
            authenticationManager = An<IAuthenticationManager>();
            command = new LoginCommand(username, password);
            handler = new LoginCommandHandler(authenticationManager);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command,metadata);
        };

        It should_use_the_authentication_manager_to_log_the_user_in = () =>
        {
            authenticationManager.WasToldTo(x => x.Login(Param.Is(username), Param.Is(password), Param.IsAny<string>()));
        };
    }
}