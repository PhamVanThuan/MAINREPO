using Capitec.Core.Identity;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Capitec.CommandHandlers;

using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Services;
using SAHL.Services.Capitec.Managers.Security;
using SAHL.Core.Data;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.AutoLoginCommandHandlerSpecs
{
    public class when_told_to_login_a_user_with_an_inactive_branch : WithAutoLoginCommandFakes
    {
        private static string branchCode;
        private static BranchDataModel branch;
        static ServiceRequestMetadata metadata;
        private static IUnitOfWorkFactory unitOfWorkFactory;

        Establish context = () =>
            {
                branchCode = "1234";
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
                branch = new BranchDataModel("InactiveBranch", Guid.NewGuid(), false, branchCode);
                securityDataServices.WhenToldTo(x => x.GetBranchByBranchCode(branchCode)).Return(branch);
            };

        Because of = () =>
            {
                messages = commandHandler.HandleCommand(command,metadata);
            };

        It should_get_branch_by_code = () =>
            {
                securityDataServices.WasToldTo(x => x.GetBranchByBranchCode(Param<string>.Matches(p => p == branchCode)));
            };

        It should_have_message_saying_branch_not_found = () =>
            {
                messages.AllMessages.ShouldContain((message) => message.Message == "Branch is not active.");
            };
    }
}
