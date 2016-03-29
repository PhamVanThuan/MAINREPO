using Capitec.Core.Identity;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.CommandHandlers;
using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Linq;
using Identity = Capitec.Core.Identity;
using SAHL.Core.Services;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.ChangePasswordCommandHandlerSpecs
{
    public class when_told_to_change_a_password_and_the_capitec_identity_userID_is_empty : WithFakes
    {
        static IUserManager userManager;
        static IHostContext hostContext;
        static ChangePasswordCommand command;
        static ChangePasswordCommandHandler handler;
        static System.Security.Principal.IPrincipal principal;
        static System.Security.Principal.IIdentity identity;
        static string password, passwordConfirmation;
        static ISystemMessageCollection messages;
        static string[] roles;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            roles = new string[]{ "roles" };
            userManager = An<IUserManager>();
            hostContext = An<IHostContext>();
            identity = new Identity.CapitecIdentity(true, "name", Guid.Empty, "name");
            principal = new GenericPrincipal(identity, roles);
            hostContext.WhenToldTo(x => x.GetUser()).Return(principal);
            command = new ChangePasswordCommand(password, passwordConfirmation);
            handler = new ChangePasswordCommandHandler(userManager, hostContext);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command,metadata);
        };

        It should_not_change_the_password = () =>
        {
            userManager.WasNotToldTo(x => x.ChangePassword(Param.IsAny<Guid>(), Param.IsAny<string>()));
        };

        It should_return_a_message_indicating_the_user_id_is_invalid = () =>
        {
            messages.AllMessages.First().Message.ShouldEqual("Bad Guid");
        };
    }
}
