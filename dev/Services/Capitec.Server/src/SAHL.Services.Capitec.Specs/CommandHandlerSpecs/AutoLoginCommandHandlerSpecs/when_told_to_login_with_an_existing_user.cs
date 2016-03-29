using Capitec.Core.Identity;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Services.Capitec.CommandHandlers;

using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Services;
using SAHL.Services.Capitec.Managers.Security;
using SAHL.Core.Data;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.AutoLoginCommandHandlerSpecs
{
    public class when_told_to_login_with_an_existing_user : WithAutoLoginCommandFakes
    {
        private static string userName;
        private static string branchCode;
        private static string password;
        private static Guid userId;
        static ServiceRequestMetadata metadata;
        private static IUnitOfWorkFactory unitOfWorkFactory;

        Establish context = () =>
            {
                authenticationManager = An<IAuthenticationManager>();
                securityDataServices = An<ISecurityDataManager>();
                userDataManager = An<IUserDataManager>();
                securityManager = An<ISecurityManager>();
                passwordManager = An<IPasswordManager>();
                unitOfWorkFactory = An<IUnitOfWorkFactory>();
                userName = "username";
                branchCode = "1234";
                password = "password";
                userId = Guid.NewGuid();
                commandHandler = new AutoLoginCommandHandler(authenticationManager,
                    securityDataServices,
                    userDataManager,
                    securityManager,
                    passwordManager,
                    unitOfWorkFactory);

                securityDataServices
                    .WhenToldTo(p => p.GetBranchByBranchCode(branchCode))
                    .Return(new BranchDataModel("branch", Guid.NewGuid(), true, branchCode));

                userDataManager
                    .WhenToldTo(p => p.DoesUsernameExist(Param<string>.Matches(m => m == userName)))
                    .Return(true);

                userDataManager
                    .WhenToldTo(p => p.GetUserFromUsername(Param<string>.Matches(m => m == userName)))
                    .Return(new UserDataModel(userName, "hash", userId, true, false));
            };

        Because of = () =>
            {
                messages = commandHandler.HandleCommand(new AutoLoginCommand(userName, branchCode, password),metadata);
            };

        It should_get_branch_by_name = () =>
            {
                securityDataServices.WasToldTo(x => x.GetBranchByBranchCode(branchCode));
                messages.AllMessages.ShouldBeEmpty();
            };

        It should_check_if_username_exists = () =>
            {
                userDataManager.WasToldTo(x => x.DoesUsernameExist(userName));
            };

        It should_get_the_user_from_username = () =>
            {
                userDataManager.WasToldTo(x => x.GetUserFromUsername(Param<string>.Matches(p => p == userName)));
            };

        It should_log_the_user_in = () =>
            {
                authenticationManager.WasToldTo(
                    x => x.Login(
                        Param<string>.Matches(m => m == userName),
                        Param<string>.Matches(m => m == password),
                        Param<string>.Matches(m => m == "")
                    )
                );
            };
    }
}
