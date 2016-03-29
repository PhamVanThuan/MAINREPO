using Capitec.Core.Identity;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.CommandHandlers;
using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Identity = Capitec.Core.Identity;
using SAHL.Core.Services;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.ChangePasswordCommandHandlerSpecs
{
    public class when_told_to_change_a_password : WithFakes
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
        static Guid id;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            id = Guid.NewGuid();
            password = "134567";
            passwordConfirmation = "123467";
            roles = new string[] { "roles" };
            userManager = An<IUserManager>();
            hostContext = An<IHostContext>();
            identity = new Identity.CapitecIdentity(true, "name", id, "name");
            principal = new GenericPrincipal(identity, roles);
            hostContext.WhenToldTo(x => x.GetUser()).Return(principal);
            command = new ChangePasswordCommand(password, passwordConfirmation);
            handler = new ChangePasswordCommandHandler(userManager, hostContext);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command,metadata);
        };

        It should_change_the_password = () =>
        {
            userManager.WasToldTo(x => x.ChangePassword(Param.Is(id), password));
        };
    }
}