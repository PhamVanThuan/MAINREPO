using Capitec.Core.Identity;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.CommandHandlers;
using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using System.Security.Principal;
using SAHL.Core.Services;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.ChangePasswordCommandHandlerSpecs
{
    public class when_told_to_change_a_password_and_no_capitec_identity_exists : WithFakes
    {
        static IUserManager userManager;
        static IHostContext hostContext;
        static ChangePasswordCommand command;
        static ChangePasswordCommandHandler handler;
        static IPrincipal identity;
        static string password, passwordConfirmation;
        static ISystemMessageCollection messages;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            userManager = An<IUserManager>();
            hostContext = An<IHostContext>();
            identity = An<IPrincipal>();
            hostContext.WhenToldTo(x => x.GetUser()).Return(identity);
            command = new ChangePasswordCommand(password, passwordConfirmation);
            handler = new ChangePasswordCommandHandler(userManager, hostContext);
        };

        Because of = () =>
        {
            handler.HandleCommand(command,metadata);
        };

        It should_not_change_the_password = () =>
        {
            userManager.WasNotToldTo(x => x.ChangePassword(Param.IsAny<Guid>(), Param.IsAny<string>()));
        };
    }
}
