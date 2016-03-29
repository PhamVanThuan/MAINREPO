using Capitec.Core.Identity;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.CommandHandlers;
using SAHL.Services.Capitec.Managers.Security;

using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Services;
using SAHL.Core.Data;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.AutoLoginCommandHandlerSpecs
{
    public class when_told_to_login_a_user_with_an_unknown_branch : WithAutoLoginCommandFakes
    {
        private static string branchCode;
        static ServiceRequestMetadata metadata;
        private static IUnitOfWorkFactory unitOfWorkFactory;

        Establish context = () =>
            {
                branchCode = "does-not-exit";
                authenticationManager = An<IAuthenticationManager>();
                securityDataServices = An<ISecurityDataManager>();
                userDataManager = An<IUserDataManager>();
                securityManager = An<ISecurityManager>();
                passwordManager = An<IPasswordManager>();
                unitOfWorkFactory = An<IUnitOfWorkFactory>();
                command = new AutoLoginCommand("cp12345", branchCode, "12345");
                commandHandler = new AutoLoginCommandHandler(authenticationManager,
                    securityDataServices,
                    userDataManager,
                    securityManager,
                    passwordManager,
                    unitOfWorkFactory);
            };

        Because of = () =>
            {
                messages = commandHandler.HandleCommand(command,metadata);
            };

        It should_get_branch_by_name = () =>
            {
                securityDataServices.WasToldTo(x => x.GetBranchByBranchCode(Param<string>.Matches(p => p == branchCode)));
            };

        It should_have_message_saying_branch_not_found = () =>
            {
                messages.AllMessages.ShouldContain((message) => message.Message == "Branch does not exist.");
            };
    }
}
